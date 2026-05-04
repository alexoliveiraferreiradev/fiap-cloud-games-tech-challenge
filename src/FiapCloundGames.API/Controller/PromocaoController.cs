using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IMapper _mapper;
        public PromocaoController(ILogger<PromocaoController> logger, IJogosService jogoService, IMapper mapper)
        {
            _logger = logger;
            _jogoService = jogoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemJogosComPromocao()
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos = await _jogoService.ObtemJogosPromovidos();
            if (jogos == null)
                return NotFound("Não foi encontrado nenhum jogo com promoções");

            return Ok(_mapper.Map<IEnumerable<JogoResponse>>(jogos));
        }
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PromocaoResponse>> ObtemPromocaoPorId(Guid id)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var promocao = await _jogoService.ObtemPromocaoPorId(id);
            if (promocao == null)
                return NotFound("Não foi encotrada nenhuma promocao com este ID");
           
            var jogo = await _jogoService.ObtemJogoPorId(promocao.JogoId);

            var promocaResponse = _mapper.Map<PromocaoResponse>(promocao);
            _mapper.Map(jogo, promocaResponse); 
            return Ok(promocaResponse);
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
