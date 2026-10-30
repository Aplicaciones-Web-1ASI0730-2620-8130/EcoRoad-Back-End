using System.Text.Json.Serialization;
using EcoRoad.Commercial.Application.CommandServices;
using EcoRoad.Commercial.Application.Internal.CommandServices;
using EcoRoad.Commercial.Application.Internal.QueryServices;
using EcoRoad.Commercial.Application.QueryServices;
using EcoRoad.Commercial.Domain.Repositories;
using EcoRoad.Commercial.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
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

builder.Services.AddScoped<ICompanyAccountRepository, CompanyAccountRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ICommercialCommandService, CommercialCommandService>();
builder.Services.AddScoped<ICommercialQueryService, CommercialQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
