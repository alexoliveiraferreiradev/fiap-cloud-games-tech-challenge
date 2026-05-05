using FiapCloundGames.API.Configuration.Extensions;
using FiapCloundGames.API.Configuration.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiConfiguration()
       .AddDependecyInjectionConfiguration()
       .AddSwaggerConfiguration();


var app = builder.Build();

app.AddMigrationSeNaoExiste().
    EnviromentConfiguration()
    .AddAppConfiguration();

app.Run();
