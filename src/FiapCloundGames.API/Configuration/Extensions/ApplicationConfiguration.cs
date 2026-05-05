using FiapCloundGames.API.Configuration.Middlewares;

namespace FiapCloundGames.API.Configuration.Extensions
{
    public static class ApplicationConfiguration
    {

        public static WebApplication EnviromentConfiguration(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fiap Cloud Games V1");
                });
            }

            return app;
        }

        public static WebApplication AddAppConfiguration(this WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.MapControllers();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHttpsRedirection();
            return app;
        }
    }
}
