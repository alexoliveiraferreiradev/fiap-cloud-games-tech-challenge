using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Configuration.Worker;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.API.Infrastructure.Security;
using NuGet.Protocol.Resources;

namespace FiapCloundGames.API.Configuration.Extensions
{
    public static class DependecyInjectionConfiguration
    {
        public static WebApplicationBuilder AddDependecyInjectionConfiguration(this WebApplicationBuilder builder)
        {
            builder.Services.AddHostedService<PromocaoCleanupWorker>();
            builder.Services.AddScoped<ApplicationDbContext>();
            builder.Services.AddScoped<IToken, TokenConfiguration>();
            builder.Services.AddScoped<IUsuarioService, UsuarioService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
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
