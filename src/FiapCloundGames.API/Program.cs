using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Endpoints;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using FiapCloundGames.API.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApplicationDbContext>();
builder.Services.AddScoped<IJogoRepository, JogoRepository>();
builder.Services.AddScoped<IJogosService, JogosService>();

var app = builder.Build();
app.AddJogoEndpoint();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fiap Cloud Games V1");
    });
}

app.UseHttpsRedirection();
app.Run();
