using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiapCloundGames.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "AdminRole")]
    [Tags("Gerenciamento de Usuários")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuarioController> _logger;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService,
            ILogger<UsuarioController> logger, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UsuarioResponse>> ObtemTodos()
        {
            return _mapper.Map<IEnumerable<UsuarioResponse>>(await _usuarioService.ObterTodos());
        }

        [HttpPut("promover-para-admin/{id:guid}")]
        public async Task<IActionResult> PromoverParaAdmin(Guid id)
        {
            var usuario = await _usuarioService.PromoverParaAdmin(id);
            return NoContent();
        }

        [HttpPut("rebaixar-para-jogador/{id:guid}")]
        public async Task<IActionResult> RebaixarParaJogador(Guid id)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();

            var currentUserId = Guid.Parse(currentUserIdClaim);
            if (id == currentUserId)
            {
                return BadRequest("Você não possui permissão para rebaixar a própria conta enquanto logado");
            }

            var usuario = await _usuarioService.RebaixarParaJogador(id,currentUserId);
            return NoContent();
        }

        [HttpPut("reativar-jogador/{id:guid}")]
        public async Task<IActionResult> ReativarJogador(Guid id)
        {
            await _usuarioService.Reativar(id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DesativarUsuario(Guid id, MotivoExclusao motivoDesativacao)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();

            var currentUserId = Guid.Parse(currentUserIdClaim);
            if (id == currentUserId)
            {
                return BadRequest("Você não possui permissão para rebaixar a própria conta enquanto logado");
            }
            var deleteRequest = new DesativaUsuarioRequest { Id =  id , MotivoDelecao = motivoDesativacao};

            await _usuarioService.Desativar(deleteRequest, currentUserId);
            return NoContent();
        }
    }
}
