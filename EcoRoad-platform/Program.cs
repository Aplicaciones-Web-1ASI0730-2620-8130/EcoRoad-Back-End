using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using EcoRoad.Alerting.Application.CommandServices;
using EcoRoad.Alerting.Application.Internal.CommandServices;
using EcoRoad.Alerting.Application.Internal.QueryServices;
using EcoRoad.Alerting.Application.QueryServices;
using EcoRoad.Alerting.Domain.Repositories;
using EcoRoad.Alerting.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations();
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddScoped<IDeviationAssessmentRepository, DeviationAssessmentRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<IAlertingCommandService, AlertingCommandService>();
builder.Services.AddScoped<IAlertingQueryService, AlertingQueryService>();
builder.Services.AddScoped<AlertingSampleDataSeeder>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
