using FiapCloudGames.Application.Services;
using FiapCloudGames.Application.Services.Interfaces;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Persistance;
using FiapCloudGames.Infrastructure.Repository;
using FiapCloudGames.Infrastructure.Security;
using FiapCloudGames.Infrastructure.Worker;

namespace FiapCloudGames.API.Extensions
{
    public static class DependecyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependecyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<PromocaoCleanupWorker>();
            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasher>();
            builder.Services.AddScoped<IPedidoService, PedidoService>();
            builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
            builder.Services.AddScoped<IBibliotecaService, BibliotecaService>();
            builder.Services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
            builder.Services.AddScoped<IJogoRepository, JogoRepository>();
            builder.Services.AddScoped<IJogosService, JogosService>();
            return builder;
        }
    }
}
