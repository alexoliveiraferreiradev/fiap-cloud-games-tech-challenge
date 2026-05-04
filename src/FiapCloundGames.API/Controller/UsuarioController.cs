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
        }

        /// <summary>
        /// Recupera a lista completa de todos os usuários cadastrados na plataforma.
        /// </summary>
        /// <remarks>
        /// Este endpoint deve ser restrito a usuários com perfil administrativo. 
        /// Retorna dados básicos de perfil, excluindo informações sensíveis.
        /// </remarks>
        /// <response code="200">Sucesso. Retorna a lista de usuários.</response>
        /// <response code="401">Não autorizado. Token inválido ou ausente.</response>
        /// <response code="403">Proibido. O usuário logado não possui permissão de administrador.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UsuarioResponse>>> ObtemTodos()
        {
            _logger.LogInformation("Solicitação de listagem geral de usuários recebida.");

            var usuarios = await _usuarioService.ObterTodos();

            if (usuarios == null || !usuarios.Any())
            {
                _logger.LogInformation("Consulta finalizada. A base de usuários está vazia.");
                return Ok(Enumerable.Empty<UsuarioResponse>());
            }
                       
            _logger.LogInformation("Listagem de usuários realizada com sucesso. Total de registros: {QuantidadeUsuarios}",
                usuarios.Count());

            return Ok(usuarios);
        }

        /// <summary>
        /// Eleva o nível de acesso de um usuário para Administrador.
        /// </summary>
        /// <param name="id">ID do usuário a ser promovido.</param>
        /// <response code="204">Sucesso. O cargo foi atualizado.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [HttpPut("promover-para-admin/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PromoverParaAdmin(Guid id)
        {
            _logger.LogInformation("Solicitação de promoção para Administrador recebida. TargetUserId: {TargetUserId}", id);

            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            var usuario = await _usuarioService.PromoverParaAdmin(id);

            _logger.LogWarning("SEGURANÇA: Usuário {TargetUserId} promovido a Administrador. Ação realizada por: {AdminId}",
         id, currentUserIdClaim ?? "Sistema/Manual");
            return NoContent();
        }

        /// <summary>
        /// Altera o perfil de um Administrador para Jogador comum.
        /// </summary>
        /// <remarks>
        /// **Regra de Segurança:** Não é permitido que um administrador rebaixe a própria conta enquanto estiver logado 
        /// para evitar a perda acidental de acesso administrativo ao sistema.
        /// </remarks>
        /// <param name="id">ID do administrador a ser rebaixado.</param>
        /// <response code="204">Sucesso. O cargo foi alterado para Jogador.</response>
        /// <response code="400">Operação inválida. Tentativa de auto-rebaixamento.</response>
        [HttpPut("rebaixar-para-jogador/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Reativa uma conta de usuário que foi anteriormente desativada.
        /// </summary>
        /// <param name="id">ID do usuário a ser reativado.</param>
        /// <response code="204">Sucesso. A conta está ativa novamente.</response>
        [HttpPut("reativar-jogador/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ReativarJogador(Guid id)
        {
            _logger.LogInformation("Solicitação para rebaixar usuário para Jogador recebida. TargetUserId: {TargetUserId}", id);
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(currentUserIdClaim))
            {
                _logger.LogWarning("Tentativa de rebaixamento sem identificação de usuário (Claim ausente). TargetUserId: {TargetUserId}", id);
                return Unauthorized();
            }

            var currentUserId = Guid.Parse(currentUserIdClaim);

            if (id == currentUserId)
            {                
                _logger.LogWarning("Operação bloqueada: Administrador {AdminId} tentou rebaixar a própria conta.", currentUserId);
                return BadRequest("Você não possui permissão para rebaixar a própria conta enquanto logado");
            }

            var usuario = await _usuarioService.RebaixarParaJogador(id, currentUserId);
            
            _logger.LogWarning("SEGURANÇA: Usuário {TargetUserId} rebaixado para Jogador. Ação realizada pelo Administrador: {AdminId}",
                id, currentUserId);

            return NoContent();
        }

        /// <summary>
        /// Desativa a conta de um usuário informando o motivo.
        /// </summary>
        /// <remarks>
        /// Esta operação exige um motivo formal para a desativação (Enum MotivoExclusao). 
        /// Administradores não podem desativar a própria conta através deste endpoint.
        /// </remarks>
        /// <param name="id">ID do usuário a ser desativado.</param>
        /// <param name="motivoDesativacao">Código do motivo (ex: 1 para Violação de Termos).</param>
        /// <response code="204">Sucesso. Usuário desativado com sucesso.</response>
        /// <response code="400">Operação inválida. Tentativa de auto-exclusão.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DesativarUsuario(Guid id, MotivoExclusao motivoDesativacao)
        {
            _logger.LogInformation("Solicitação administrativa para desativar usuário recebida. TargetUserId: {TargetUserId}, Motivo: {Motivo}", id, motivoDesativacao);
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim))
            {
                _logger.LogWarning("Tentativa de desativação administrativa sem identificação (Claim ausente). TargetUserId: {TargetUserId}", id);
                return Unauthorized();
            }

            if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {
                _logger.LogWarning("Falha ao processar identificação do administrador. Claim inválida: {UserClaim}", currentUserIdClaim);
                return Unauthorized();
            }

            if (id == currentUserId)
            {                
                _logger.LogWarning("Operação bloqueada: Administrador {AdminId} tentou desativar a própria conta administrativa.", currentUserId);
                return BadRequest("Você não possui permissão para desativar a própria conta enquanto logado.");
            }

            var deleteRequest = new DesativaUsuarioRequest { Id = id, MotivoDelecao = motivoDesativacao };

            await _usuarioService.Desativar(deleteRequest, currentUserId);

            // Log de auditoria: Registro definitivo da ação administrativa
            _logger.LogWarning("SEGURANÇA: Usuário {TargetUserId} desativado pelo Administrador {AdminId}. Motivo Registrado: {Motivo}",
                id, currentUserId, motivoDesativacao);
            return NoContent();
        }
    }
}
