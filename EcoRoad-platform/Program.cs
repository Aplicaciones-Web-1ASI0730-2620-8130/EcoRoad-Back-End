using System.Reflection;
using System.Text.Json.Serialization;
using EcoRoad.Assets.Application.Acl;
using EcoRoad.Assets.Application.CommandServices;
using EcoRoad.Assets.Application.Internal.CommandServices;
using EcoRoad.Assets.Application.Internal.QueryServices;
using EcoRoad.Assets.Application.QueryServices;
using EcoRoad.Assets.Domain.Repositories;
using EcoRoad.Assets.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Assets.Interfaces.Acl;
using EcoRoad.Iam.Application.Acl;
using EcoRoad.Iam.Application.CommandServices;
using EcoRoad.Iam.Application.Internal.CommandServices;
using EcoRoad.Iam.Application.Internal.OutboundServices;
using EcoRoad.Iam.Application.Internal.QueryServices;
using EcoRoad.Iam.Application.QueryServices;
using EcoRoad.Iam.Domain.Repositories;
using EcoRoad.Iam.Infrastructure.Hashing.BCrypt.Services;
using EcoRoad.Iam.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Iam.Infrastructure.Pipeline.Middleware.Extensions;
using EcoRoad.Iam.Infrastructure.Tokens.Jwt.Configuration;
using EcoRoad.Iam.Infrastructure.Tokens.Jwt.Services;
using EcoRoad.Iam.Interfaces.Acl;
using EcoRoad.Incidents.Application.CommandServices;
using EcoRoad.Incidents.Application.Internal.CommandServices;
using EcoRoad.Incidents.Application.Internal.QueryServices;
using EcoRoad.Incidents.Application.QueryServices;
using EcoRoad.Incidents.Domain.Repositories;
using EcoRoad.Incidents.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
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
        Description = "EcoRoad web services (modular monolith with a bounded context per module)."
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

// Identity and Access Management bounded context dependency injection.
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("TokenSettings"));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserInvitationRepository, UserInvitationRepository>();
builder.Services.AddScoped<IHashingService, HashingService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserInvitationCommandService, UserInvitationCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserInvitationQueryService, UserInvitationQueryService>();
builder.Services.AddScoped<IIamContextFacade, IamContextFacade>();
builder.Services.AddScoped<IamSampleDataSeeder>();

// Monitoring Asset and Deployment bounded context dependency injection.
builder.Services.AddScoped<IMonitoringPointRepository, MonitoringPointRepository>();
builder.Services.AddScoped<ISensorRepository, SensorRepository>();
builder.Services.AddScoped<ISensorAssignmentRepository, SensorAssignmentRepository>();
builder.Services.AddScoped<IMonitoringPointCommandService, MonitoringPointCommandService>();
builder.Services.AddScoped<ISensorCommandService, SensorCommandService>();
builder.Services.AddScoped<IMonitoringPointQueryService, MonitoringPointQueryService>();
builder.Services.AddScoped<ISensorQueryService, SensorQueryService>();
builder.Services.AddScoped<IAssetsContextFacade, AssetsContextFacade>();
builder.Services.AddScoped<AssetsSampleDataSeeder>();

// Environmental Monitoring bounded context dependency injection.
builder.Services.AddScoped<IThresholdProfileRepository, ThresholdProfileRepository>();
builder.Services.AddScoped<IEnvironmentalMeasurementRepository, EnvironmentalMeasurementRepository>();
builder.Services.AddScoped<IThresholdProfileCommandService, ThresholdProfileCommandService>();
builder.Services.AddScoped<IEnvironmentalMeasurementCommandService, EnvironmentalMeasurementCommandService>();
builder.Services.AddScoped<IThresholdProfileQueryService, ThresholdProfileQueryService>();
builder.Services.AddScoped<IEnvironmentalMeasurementQueryService, EnvironmentalMeasurementQueryService>();
builder.Services.AddScoped<MonitoringSampleDataSeeder>();

// Incident and Remediation Management bounded context dependency injection.
builder.Services.AddScoped<IEnvironmentalIncidentRepository, EnvironmentalIncidentRepository>();
builder.Services.AddScoped<IEnvironmentalIncidentCommandService, EnvironmentalIncidentCommandService>();
builder.Services.AddScoped<ICorrectiveActionCommandService, CorrectiveActionCommandService>();
builder.Services.AddScoped<IEnvironmentalIncidentQueryService, EnvironmentalIncidentQueryService>();
builder.Services.AddScoped<IncidentsSampleDataSeeder>();

var app = builder.Build();

// Apply pending migrations and, in development, load the demo data.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();

    if (app.Environment.IsDevelopment())
    {
        var iamSeeder = scope.ServiceProvider.GetRequiredService<IamSampleDataSeeder>();
        await iamSeeder.SeedIfEmptyAsync();

        var assetsSeeder = scope.ServiceProvider.GetRequiredService<AssetsSampleDataSeeder>();
        await assetsSeeder.SeedIfEmptyAsync();

        var monitoringSeeder = scope.ServiceProvider.GetRequiredService<MonitoringSampleDataSeeder>();
        await monitoringSeeder.SeedIfEmptyAsync();

        var incidentsSeeder = scope.ServiceProvider.GetRequiredService<IncidentsSampleDataSeeder>();
        await incidentsSeeder.SeedIfEmptyAsync();
    }
}

// The global exception handler must be the first middleware of the pipeline.
app.UseGlobalExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
app.UseRequestAuthorization();

app.MapControllers();
app.Run();
