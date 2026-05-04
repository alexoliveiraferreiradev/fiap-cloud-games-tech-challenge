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

        /// <summary>
        /// Recupera o catálogo de jogos que possuem ofertas ativas, de forma paginada.
        /// </summary>
        /// <remarks>
        /// Antes de listar, o sistema executa uma limpeza automática de promoções expiradas 
        /// para garantir que apenas ofertas válidas sejam exibidas na vitrine.
        /// </remarks>
        /// <response code="200">Sucesso. Retorna a lista de jogos promovidos.</response>
        /// <response code="404">Nenhuma promoção ativa encontrada no momento.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagedResult<JogoResponse>>> ObtemJogosComPromocao()
        {
            _logger.LogInformation("Iniciando consulta de jogos em promoção.");
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos = await _jogoService.ObtemJogosPromovidosPaginacao();
            if ( jogos == null|| !jogos.Items.Any())
            {
                _logger.LogInformation("Consulta finalizada. Nenhuma promoção ativa encontrada no momento.");
                return NotFound("Não foi encontrado nenhum jogo com promoções");
            }
            _logger.LogInformation("Promoções recuperadas com sucesso. Quantidade de Jogos: {QuantidadeJogos}",jogos.Items.Count());
            return Ok(jogos);
        }
        /// <summary>
        /// Obtém informações detalhadas de uma promoção específica através do seu ID.
        /// </summary>
        /// <param name="id">Identificador único (GUID) da promoção.</param>
        /// <response code="200">Sucesso. Retorna os dados da promoção e os detalhes do jogo vinculado.</response>
        /// <response code="404">Promoção não encontrada ou já expirada.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PromocaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PromocaoResponse>> ObtemPromocaoPorId(Guid id)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var promocaoResponse = await _jogoService.ObtemPromocaoPorId(id);
            if (promocaoResponse == null)
                return NotFound("Não foi encotrada nenhuma promocao com este ID");           
           
            return Ok(promocaoResponse);
        }

        /// <summary>
        /// Registra uma nova oferta para um jogo específico no sistema.
        /// </summary>
        /// <remarks>
        /// O valor da promoção deve ser inferior ao preço original do jogo. 
        /// A data de expiração define quando a oferta deixará de ser listada automaticamente.
        /// </remarks>
        /// <param name="promocaoRequest">Dados da nova promoção (JogoId, Valor e Data Fim).</param>
        /// <response code="201">Criado. Promoção registrada com sucesso.</response>
        /// <response code="400">Dados inválidos ou regra de negócio violada (ex: jogo não existe).</response>
        [HttpPost("/nova-promocao")]
        [ProducesResponseType(typeof(PromocaoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> CriaPromocao(CriaPromocaoRequest promocaoRequest)
        {
            await _jogoService.AdicionarPromocao(promocaoRequest);
            return CreatedAtAction(nameof(ObtemPromocaoPorId), new { id = promocaoRequest.JogoId }, promocaoRequest);
        }

        /// <summary>
        /// Encerra antecipadamente uma promoção ativa.
        /// </summary>
        /// <param name="id">O identificador único da promoção a ser desativada.</param>
        /// <response code="204">Sucesso. A promoção foi removida do catálogo de ofertas.</response>
        [HttpDelete("/desativar-promocao/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DesativarPromocao(Guid id)
        {
            await _jogoService.DesativarPromocao(id);
            return NoContent();
        }

    }
}
