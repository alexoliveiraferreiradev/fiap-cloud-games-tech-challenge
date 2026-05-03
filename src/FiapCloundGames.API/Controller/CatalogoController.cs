using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        public CatalogoController(ILogger<CatalogoController> logger, IJogosService jogosService, IMapper mapper)
        {
            _logger = logger;
            _jogoService = jogosService;
            _mapper = mapper;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            _logger.LogInformation("Obtém jogo por id: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo is null) return NotFound();

            return Ok(_mapper.Map<JogoResponse>(jogo));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemCatalogoDeJogos()
        {
            _logger.LogInformation("Recupera catálogo de jogos");
            var jogos = await _jogoService.ObtemCatalagoJogos();
            if (!jogos.Any()) return NoContent();
            return Ok(jogos);
        }
    }
}
