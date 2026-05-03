using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Repositories;

namespace FiapCloundGames.API.Endpoints
{
    public class JogoEndpointsLog { };
    public static class JogoEndpoints
    {
        public static WebApplication AddJogoEndpoint(this WebApplication app)
        {
            var catalagoGroup = app.MapGroup("/catalogo-jogos").WithTags("Catálogo de jogos");

            catalagoGroup.MapGet("/", async (IJogosService _jogosService,ILogger<JogoEndpointsLog> logger) =>
            {
                logger.LogInformation("Recupera catálogo de jogos");
                var jogos = await _jogosService.ObtemCatalagoJogos();
                return jogos.Any() ? Results.Ok(jogos) : Results.NoContent();
            }).AllowAnonymous();

            var gamesGroup = app.MapGroup("/jogos").WithTags("Jogos");

            gamesGroup.MapGet("{id:guid}/", async (Guid id,IJogosService _jogosService, ILogger<JogoEndpointsLog> logger) =>
            {
                logger.LogInformation("Obtém jogo por id");
                var jogo = await _jogosService.ObtemJogoPorId(id);
                return jogo!=null ? Results.Ok(jogo) : Results.NotFound();
            });

            return app;
        }
    }
}
