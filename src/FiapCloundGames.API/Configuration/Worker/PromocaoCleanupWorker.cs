using FiapCloundGames.API.Application.Services.Interfaces;

namespace FiapCloundGames.API.Configuration.Worker
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
