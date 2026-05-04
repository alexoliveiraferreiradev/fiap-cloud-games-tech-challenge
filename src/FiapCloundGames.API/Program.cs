using FiapCloundGames.API.Configuration.Extensions;
using FiapCloundGames.API.Configuration.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.AddApiConfiguration()
       .AddDependecyInjectionConfiguration()
       .AddSwaggerConfiguration();


var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fiap Cloud Games V1");
    });
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.Run();
