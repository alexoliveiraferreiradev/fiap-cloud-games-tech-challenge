using FiapCloudGames.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiConfiguration().AddSwaggerConfiguration();


var app = builder.Build();

app.EnviromentConfiguration()
    .AddAppConfiguration();

app.Run();
