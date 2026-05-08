using FiapCloudGames.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FiapCloudGames.Infrastructure.Worker
{
    public class PromocaoCleanupWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public PromocaoCleanupWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var jogoService = scope.ServiceProvider.GetRequiredService<IJogosService>();
                    await jogoService.DesativaPromocoesInvalidas();
                }
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
