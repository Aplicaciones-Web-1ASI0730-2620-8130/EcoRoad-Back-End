using System.Reflection;
using System.Text.Json.Serialization;
using EcoRoad.Monitoring.Application.CommandServices;
using EcoRoad.Monitoring.Application.Internal.CommandServices;
using EcoRoad.Monitoring.Application.Internal.QueryServices;
using EcoRoad.Monitoring.Application.QueryServices;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Shared.Domain.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Shared.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Shared.Interfaces.Rest.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Controllers: enums travel as strings and broken domain rules become error resources.
builder.Services.AddControllers(options => options.Filters.Add<DomainRuleExceptionFilter>())
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// The web application is deployed on another origin, so cross origin requests are allowed.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// OpenAPI documentation enriched with the XML comments of the project.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EcoRoad API",
        Version = "v1",
        Description = "EcoRoad web services: Environmental Monitoring bounded context."
    });

    var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (File.Exists(xmlFile))
    {
        options.IncludeXmlComments(xmlFile);
    }
});

// Database context.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Shared kernel dependency injection.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Environmental Monitoring bounded context dependency injection.
builder.Services.AddScoped<IThresholdProfileRepository, ThresholdProfileRepository>();
builder.Services.AddScoped<IEnvironmentalMeasurementRepository, EnvironmentalMeasurementRepository>();
builder.Services.AddScoped<IThresholdProfileCommandService, ThresholdProfileCommandService>();
builder.Services.AddScoped<IEnvironmentalMeasurementCommandService, EnvironmentalMeasurementCommandService>();
builder.Services.AddScoped<IThresholdProfileQueryService, ThresholdProfileQueryService>();
builder.Services.AddScoped<IEnvironmentalMeasurementQueryService, EnvironmentalMeasurementQueryService>();
builder.Services.AddScoped<MonitoringSampleDataSeeder>();

var app = builder.Build();

// Apply pending migrations and, in development, load the demo data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (app.Environment.IsDevelopment())
    {
        var monitoringSeeder = scope.ServiceProvider.GetRequiredService<MonitoringSampleDataSeeder>();
        await monitoringSeeder.SeedIfEmptyAsync();
    }
}

// The global exception handler must be the first middleware of the pipeline.
app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();
app.Run();
