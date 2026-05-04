using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "AdminRole")]
    [Tags("Gerenciamento de Promoções")]
    public class PromocaoController : ControllerBase
    {
        private readonly ILogger<PromocaoController> _logger;
        private readonly IJogosService _jogoService;
        public PromocaoController(ILogger<PromocaoController> logger, IJogosService jogoService)
        {
            _logger = logger;
            _jogoService = jogoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemJogosComPromocao()
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos = await _jogoService.ObtemJogosPromovidos();
            if (jogos == null)
                return NotFound("Não foi encontrado nenhum jogo com promoções");

            return Ok(jogos);
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PromocaoResponse>> ObtemPromocaoPorId(Guid id)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var promocaoResponse = await _jogoService.ObtemPromocaoPorId(id);
            if (promocaoResponse == null)
                return NotFound("Não foi encotrada nenhuma promocao com este ID");           
           
            return Ok(promocaoResponse);
        }

        [HttpPost("/nova-promocao")]
        public async Task<ActionResult<JogoResponse>> CriaPromocao(CriaPromocaoRequest promocaoRequest)
        {
            await _jogoService.AdicionarPromocao(promocaoRequest);
            return CreatedAtAction(nameof(ObtemPromocaoPorId), new { id = promocaoRequest.JogoId }, promocaoRequest);
        }

        [HttpDelete("/desativar-promocao/{id:guid}")]
        public async Task<ActionResult> DesativarPromocao(Guid id)
        {
            await _jogoService.DesativarPromocao(id);
            return NoContent();
        }

    }
}
