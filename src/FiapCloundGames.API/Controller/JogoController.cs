using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Gerenciamento de Jogos")]
    [Authorize(Roles = "AdminRole")]
    public class JogoController : ControllerBase
    {
        private readonly ILogger<JogoController> _logger;
        private readonly IJogosService _jogoService;
        private readonly IMapper _mapper;
        public JogoController(ILogger<JogoController> logger, IJogosService jogosService, IMapper mapper)
        {
            _logger = logger;
            _jogoService = jogosService;
            _mapper = mapper;
        }
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            _logger.LogInformation("Obtém jogo por id: {Id}", id);
            var jogo = _mapper.Map<JogoResponse>(  await _jogoService.ObtemJogoPorId(id));
            if (jogo is null) return NotFound();

            return Ok(_mapper.Map<JogoResponse>(jogo));
        }
        [HttpPost]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> Adicionar(CriarJogoRequest jogoRequest)
        {
            _logger.LogInformation("Recebida requisição para adicionar o jogo: {NomeJogo}", jogoRequest.Nome);

            var jogo = _mapper.Map<JogoResponse>( await _jogoService.AdicionaJogo(jogoRequest));
            return  CreatedAtAction(nameof(ObterJogoPorId), new { id = jogo.Id }, jogo);
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Desativar(Guid id)
        {
            _logger.LogInformation("Recebida requisição para deletar jogo. ID: {Id}", id);

            await _jogoService.Desativar(id);

            _logger.LogInformation("Jogo {Id} desativado com sucesso",id);
            return NoContent();
        }

        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoResponse>> Atualizar(Guid id, UpdateJogoRequest updateRequest)
        {
            _logger.LogInformation("Recebida requisição para atualizar jogo. ID: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if(jogo == null)
                return NotFound();

            return _mapper.Map<JogoResponse>(await _jogoService.AtualizarJogo(id,updateRequest));            
        }
    }
}
