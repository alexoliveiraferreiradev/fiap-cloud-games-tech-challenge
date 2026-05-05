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

        /// <summary>
        /// Atualiza as informações do perfil do usuário.
        /// </summary>
        /// <remarks>
        /// * **Validação de Nome:** Obrigatório, entre 3 e 20 caracteres.
        /// * **Validação de E-mail:** Obrigatório, formato válido, verificação de duplicidade.
        /// Este endpoint permite a alteração de dados como nome e preferências. 
        /// 
        /// **Nota:** O ID na URL deve coincidir com o ID do recurso que se deseja atualizar.
        /// 
        /// Exemplo de requisição:
        /// 
        ///     PUT /api/usuario/3fa85f64-5717-4562-b3fc-2c963f66afa6
        ///     {
        ///        "nome": "Novo nome de usuário",
        ///        "email": "novo.dev@exemplo.com"
        ///     }
        /// </remarks>
        /// <param name="id">Identificador único do usuário.</param>
        /// <param name="updateUsuarioRequest">Novos dados para atualização.</param>
        /// <response code="200">Sucesso. Retorna os dados do usuário atualizados.</response>
        /// <response code="400">Dados inválidos ou ID da URL diferente do ID do corpo da requisição.</response>
        /// <response code="404">Usuário não encontrado.</response>
        [HttpPut]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioResponse>> AtualizarUsuario(UpdateUsuarioRequest updateUsuarioRequest)
        {
            var currentId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (string.IsNullOrEmpty(currentId))
            {                
                return Unauthorized();
            }

            var usuarioId = Guid.Parse(currentId);

            _logger.LogInformation("Iniciando atualização de perfil do usuário. UserId: {UserId}", usuarioId);

            var usuarioAtualizado = await _usuarioService.AtualizarUsuario(usuarioId, updateUsuarioRequest);

            if (usuarioAtualizado == null)
            {                
                _logger.LogInformation("Atualização cancelada. Usuário não encontrado na base de dados. UserId: {UserId}", usuarioId);
                return NotFound("Usuário não encontrado.");
            }

            _logger.LogInformation("Perfil do usuário atualizado com sucesso. UserId: {UserId}", usuarioId);

            return Ok(usuarioAtualizado);
        }

        /// <summary>
        /// Realiza a desativação lógica (soft delete) da conta do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Por motivos de segurança, um usuário só pode encerrar sua própria conta. 
        /// O sistema valida se o ID fornecido na URL é o mesmo contido no Token JWT.
        /// </remarks>
        /// <param name="id">ID da conta que será desativada.</param>
        /// <response code="204">Sucesso. A conta foi desativada (Não retorna conteúdo).</response>
        /// <response code="400">Operação inválida. Tentativa de desativar conta de terceiros.</response>
        /// <response code="401">Não autorizado. Token ausente ou inválido.</response>
        [HttpDelete("desativar-conta/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> DesativarConta(Guid id)
        {
            _logger.LogInformation("Solicitação recebida para desativar conta. TargetUserId: {TargetUserId}", id);
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {                
                _logger.LogWarning("Falha de autorização na desativação de conta. Token ausente ou inválido. TargetUserId: {TargetUserId}", id);
                return Unauthorized();
            }
            if (currentUserId != id)
            {
                _logger.LogWarning("Violação de segurança interceptada: Usuário autenticado {CurrentUserId} tentou desativar a conta do usuário {TargetUserId}", currentUserId, id);
                return BadRequest("Operação inválida: Você não possui permissão para encerrar a conta de outro usuário.");
            }

            await _usuarioService.DesativarConta(id);

            _logger.LogInformation("Conta desativada com sucesso pelo próprio usuário. UserId: {UserId}", id);
            return NoContent();
        }
    }
}
