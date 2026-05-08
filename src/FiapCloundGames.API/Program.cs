using FiapCloudGames.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiConfiguration()
       .AddDependecyInjectionConfiguration()
       .AddSwaggerConfiguration();


var app = builder.Build();

app.SeedData();

app.EnviromentConfiguration()
    .AddAppConfiguration();

app.Run();
