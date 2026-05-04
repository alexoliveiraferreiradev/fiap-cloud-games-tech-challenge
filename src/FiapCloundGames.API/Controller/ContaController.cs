using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AcessoGeral")]
    [Tags("Minha Conta")]
    public class ContaController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<ContaController> _logger;
        public ContaController(IUsuarioService usuarioService,
            ILogger<ContaController> logger)
        {         
            _usuarioService = usuarioService;
            _logger = logger;
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<UsuarioResponse>> AtualizarUsuario(Guid id, UpdateUsuarioRequest updateUsuarioRequest)
        {
            var usuarioAtualizado = await _usuarioService.AtualizarUsuario(id, updateUsuarioRequest);
            return Ok(usuarioAtualizado);
        }

        [HttpDelete("desativar-conta/{id:guid}")]
        public async Task<ActionResult> DesativarConta(Guid id)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();
            var currentUserId = Guid.Parse(currentUserIdClaim);
            if(currentUserId != id)
            {
                return BadRequest("Operação inválida: Você não possui permissão para encerrar a conta de outro usuário.");
            }
            await _usuarioService.DesativarConta(id);
            return NoContent();
        }
    }
}
