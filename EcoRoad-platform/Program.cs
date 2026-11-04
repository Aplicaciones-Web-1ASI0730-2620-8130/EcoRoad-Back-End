using System.Reflection;
using System.Text.Json.Serialization;
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
        Description = "EcoRoad web services."
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

var app = builder.Build();

// The global exception handler must be the first middleware of the pipeline.
app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.MapControllers();
app.Run();
