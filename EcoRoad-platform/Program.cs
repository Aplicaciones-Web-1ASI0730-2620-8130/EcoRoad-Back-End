using System.Text.Json.Serialization;
using EcoRoad.Compliance.Application.CommandServices;
using EcoRoad.Compliance.Application.Internal.CommandServices;
using EcoRoad.Compliance.Application.Internal.QueryServices;
using EcoRoad.Compliance.Application.QueryServices;
using EcoRoad.Compliance.Domain.Repositories;
using EcoRoad.Compliance.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddScoped<IEnvironmentalStatusRepository, EnvironmentalStatusRepository>();
builder.Services.AddScoped<IComplianceReportRepository, ComplianceReportRepository>();
builder.Services.AddScoped<IComplianceCommandService, ComplianceCommandService>();
builder.Services.AddScoped<IComplianceQueryService, ComplianceQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
