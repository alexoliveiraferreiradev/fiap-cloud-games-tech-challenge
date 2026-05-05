using FiapCloundGames.API.Application.Dtos.Pedido;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy ="AcessoGeral")]
    [Tags("Meus Pedidos")]
    public class PedidoController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(IPedidoService pedidoService, ILogger<PedidoController> logger)
        {
            _pedidoService = pedidoService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera o histórico completo de pedidos realizados pelo usuário autenticado.
        /// </summary>
        /// <remarks>
        /// A identificação do usuário é feita através do Token JWT. Se o usuário não possuir pedidos, será retornado 404.
        /// </remarks>
        /// <response code="200">Sucesso. Retorna a lista de pedidos do usuário.</response>
        /// <response code="401">Não autorizado. Token ausente ou inválido.</response>
        /// <response code="404">Nenhum pedido encontrado para este perfil.</response>
        [HttpGet("historico-pedido")]
        [ProducesResponseType(typeof(IEnumerable<PedidoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<PedidoResponse>>> ObtemHistoricoDePedidos()
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim))
            {
                _logger.LogWarning("Tentativa de acesso não autorizado ao histórico de pedidos.");
                return Unauthorized();
            }

            if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {
                _logger.LogWarning("Falha ao processar identificação do usuário. Claim inválida: {UserClaim}", currentUserIdClaim);
                return Unauthorized();
            }
            _logger.LogInformation("Consultando histórico de pedidos. UserId: {UserId}", currentUserId);
            var pedidos = await _pedidoService.ObtemHistoricoPorUsuario(currentUserId);
            if (pedidos ==null || !pedidos.Any())
            {
                _logger.LogInformation("Consulta finalizada. Nenhum pedido encontrado para o UserId: {UserId}", currentUserId);
                return NotFound("Não foi encontrado nenhum pedido para este usuário");
            }
            _logger.LogInformation("Histórico de pedidos recuperado com sucesso. UserId: {UserId}, TotalPedidos: {QuantidadePedidos}",currentUserId, pedidos.Count());
            return Ok(pedidos);
        }

        /// <summary>
        /// Recupera os detalhes de um pedido específico através do seu identificador único.
        /// </summary>
        /// <param name="id">O GUID do pedido gerado no momento da compra.</param>
        /// <response code="200">Sucesso. Retorna os detalhes do pedido solicitado.</response>
        /// <response code="404">Pedido não encontrado na base de dados.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PedidoResponse>> ObtemPedidoPorId(Guid pedidoId)
        {
            _logger.LogInformation("Consultando detalhes do pedido. PedidoId: {PedidoId}", pedidoId);
            var pedidos = await _pedidoService.ObterPedidoPorId(pedidoId);
            if (pedidos == null)
            {                
                _logger.LogInformation("Consulta finalizada. Pedido não encontrado. PedidoId: {PedidoId}", pedidoId);
                return NotFound("Não foi encontrado nenhum pedido com o ID informado.");
            }
            _logger.LogInformation("Pedido recuperado com sucesso. PedidoId: {PedidoId}, UserId: {UserId}",pedidos.Id, pedidos.UsuarioId);
            return Ok(pedidos);
        }

        /// <summary>
        /// Cria um novo pedido de compra para o usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Este endpoint processa a lista de IDs de jogos, valida preços e promoções ativas, 
        /// e gera o registro de venda vinculado ao usuário do token.
        /// 
        /// Exemplo de requisição:
        /// 
        ///     POST /api/pedidos
        ///     [
        ///       "3fa85f64-5717-4562-b3fc-2c963f66afa6",
        ///       "4ba96g75-6828-5673-c4gd-3d074g77bgb7"
        ///     ]
        /// </remarks>
        /// <param name="jogosIds">Lista de identificadores únicos dos jogos que compõem o carrinho.</param>
        /// <response code="201">Criado. O pedido foi processado e finalizado com sucesso.</response>
        /// <response code="400">Dados inválidos ou jogos inexistentes na lista fornecida.</response>
        /// <response code="401">Não autorizado. É necessário estar logado para comprar.</response>
        [HttpPost]
        [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PedidoResponse>> RealizarPedido(List<Guid> jogosIds)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();

            var currentUserId = Guid.Parse(currentUserIdClaim);
            var pedidoItemResponse = await _pedidoService.RealizarPedido(currentUserId, jogosIds);
            
            return Ok(pedidoItemResponse);
        }
    }
}
