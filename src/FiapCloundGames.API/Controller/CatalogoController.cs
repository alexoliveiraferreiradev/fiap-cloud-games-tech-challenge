using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Tags("Catálogo de Jogos")]
    public class CatalogoController : ControllerBase
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly IJogosService _jogoService;
        public CatalogoController(ILogger<CatalogoController> logger, 
            IJogosService jogosService)
        {
            _logger = logger;
            _jogoService = jogosService;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            _logger.LogInformation("Obtém jogo por id: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo is null) return NotFound();

            return Ok(jogo);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemCatalogoDeJogos()
        {
            await _jogoService.DesativaPromocoesInvalidas();
            _logger.LogInformation("Recupera catálogo de jogos");
            var jogos = await _jogoService.ObtemCatalagoJogos();
            if (!jogos.Any()) 
                return NotFound("Não foi encontrado jogos");

            return Ok(jogos);
        }

        [HttpGet("buscar-por-genero/{genero}")]
        [ProducesResponseType(typeof(IEnumerable<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ListarPorGenero(GeneroJogo genero)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos =await _jogoService.ObtemPorGenero(genero);
            if (!jogos.Any())
                return NotFound("Não foi encontrado jogos");

            return Ok(jogos);
        }

        [HttpGet("busca-jogos-com-promocao")]
        public async Task<ActionResult<JogoResponse>> ObtemJogosComPromocao()
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos = await _jogoService.ObtemJogosPromovidos();
            if (jogos == null)
                return NotFound("Não foi encontrado nenhum jogo com promoções");

            return Ok(jogos);
        }
    }
}
