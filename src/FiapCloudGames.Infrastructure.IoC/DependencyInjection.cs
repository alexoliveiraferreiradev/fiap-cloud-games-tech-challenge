using FiapCloudGames.Application.Dtos.Jogos;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Caching;
using FiapCloudGames.Infrastructure.Persistance;
using FiapCloudGames.Infrastructure.Repository;
using FiapCloudGames.Infrastructure.Security;
using FiapCloudGames.Infrastructure.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiapCloudGames.Infrastructure.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureIoC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(JogoFiltroRequest).Assembly);
            services.AddHostedService<PromocaoCleanupWorker>();
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<ITokenService, JwtTokenService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICacheService, RedisCacheService>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IPasswordHasherService, PasswordHasher>();
            services.AddScoped<IPedidoService, PedidoService>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IBibliotecaService, BibliotecaService>();
            services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();
            services.AddScoped<IJogoRepository, JogoRepository>();
            services.AddScoped<IJogosService, JogosService>();
            return services;
        }
    }
}
