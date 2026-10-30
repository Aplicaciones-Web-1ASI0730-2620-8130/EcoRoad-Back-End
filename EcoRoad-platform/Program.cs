using EcoRoad.Projects.Application.CommandServices;
using EcoRoad.Projects.Application.Internal.CommandServices;
using EcoRoad.Projects.Application.Internal.QueryServices;
using EcoRoad.Projects.Application.QueryServices;
using EcoRoad.Projects.Domain.Repositories;
using EcoRoad.Projects.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EcoRoad.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseMySQL(connectionString);
});

builder.Services.AddScoped<IRoadProjectRepository, RoadProjectRepository>();
builder.Services.AddScoped<IRoadSectionRepository, RoadSectionRepository>();
builder.Services.AddScoped<IWorkFrontRepository, WorkFrontRepository>();
builder.Services.AddScoped<IRoadSiteCommandService, RoadSiteCommandService>();
builder.Services.AddScoped<IRoadSiteQueryService, RoadSiteQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
