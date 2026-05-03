using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class JogoController : ControllerBase
    {
        private readonly ILogger<JogoController> _logger;
        private readonly IJogosService _jogoService;
        private readonly IMapper _mapper;
        public JogoController(ILogger<JogoController> logger,IJogosService jogosService,IMapper mapper ) {
            _logger = logger;
            _jogoService = jogosService;
            _mapper = mapper;
        } 

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            _logger.LogInformation("Obtém jogo por id: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo is null) return NotFound();

            var jogoResponse = _mapper.Map<JogoResponse>(jogo);

            return Ok(jogoResponse);
        }

        [Tags("Catálogo de jogos")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemCatalogoDeJogos()
        {
            _logger.LogInformation("Recupera catálogo de jogos");
            var jogos = await _jogoService.ObtemCatalagoJogos();
            return jogos.Any() ? Ok(jogos) : NoContent();
        }
    }
}
