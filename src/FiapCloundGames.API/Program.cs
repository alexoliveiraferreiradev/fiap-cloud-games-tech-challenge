using FiapCloundGames.API.Configuration.Extensions;
using FiapCloundGames.API.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiConfiguration()
       .AddDependecyInjectionConfiguration()
       .AddSwaggerConfiguration();


var app = builder.Build();

app.SeedData();

app.EnviromentConfiguration()
    .AddAppConfiguration();

app.Run();
