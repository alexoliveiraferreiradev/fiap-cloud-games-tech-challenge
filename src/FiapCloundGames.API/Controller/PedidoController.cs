using AutoMapper;
using Azure;
using FiapCloundGames.API.Application.Dtos.Pedido;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        [HttpGet("historico-pedido")]
        public async Task<ActionResult<IEnumerable<PedidoResponse>>> ObtemHistoricoDePedidos()
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();

            var currentUserId = Guid.Parse(currentUserIdClaim);
            var pedidos = await _pedidoService.ObtemHistoricoPorUsuario(currentUserId);
            if(!pedidos.Any()) 
                return NotFound("Não foi encontrado nenhum pedido para este usuário");

            return Ok(pedidos);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<PedidoResponse>> ObtemPedidoPorId(Guid pedidoId)
        {
            var pedidos = await _pedidoService.ObterPedidoPorId(pedidoId);
            if(pedidos ==null) 
                return NotFound("Não foi encontrado nenhum pedido para este usuário");

            return Ok(pedidos);
        }

        [HttpPost]
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
