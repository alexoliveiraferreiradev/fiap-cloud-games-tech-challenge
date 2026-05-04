using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy ="AcessoGeral")]
    [Tags("Minha Biblioteca")]
    public class BibliotecaController : ControllerBase
    {
        private readonly IBibliotecaService _bibliotecaService;
        private readonly ILogger<BibliotecaController> _logger;

        public BibliotecaController(IBibliotecaService bibliotecaService, 
            ILogger<BibliotecaController> logger)
        {
            _bibliotecaService = bibliotecaService;
            _logger = logger;
        }

        /// <summary>
        /// Recupera a lista de jogos pertencentes à biblioteca do usuário autenticado.
        /// </summary>
        /// <remarks>
        /// Este endpoint identifica o usuário automaticamente através do token JWT enviado no cabeçalho 'Authorization'.
        /// 
        /// **Requisito:** É necessário estar autenticado (Bearer Token).
        /// </remarks>
        /// <returns>Retorna uma lista de jogos adquiridos pelo usuário.</returns>
        /// <response code="200">Sucesso. Retorna a lista de jogos do usuário.</response>
        /// <response code="401">Não autorizado. O token JWT está ausente, expirado ou é inválido.</response>
        /// <response code="404">Nenhum jogo foi encontrado na biblioteca deste usuário.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BibliotecaResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<BibliotecaResponse>>> ObtemBibliotecaJogador([FromQuery] int pagina =1, [FromQuery] int tamanhoPagina = 10)
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(currentUserIdClaim, out var currentUserId))
            {
                _logger.LogWarning("Acesso negado na biblioteca: Claim de ID do usuário inválido ou ausente.");
                return Unauthorized("Sessão inválida ou identificação do usuário comprometida.");
            }
            _logger.LogInformation("Consultando biblioteca de jogos. UserId: {UserId}, Pagina: {Pagina}, TamanhoPagina: {TamanhoPagina}", currentUserId, pagina, tamanhoPagina);
            var jogosBiblioteca = await _bibliotecaService.ObtemBibliotecaDoUsuarioPaginacao(currentUserId,pagina,tamanhoPagina);

            if (jogosBiblioteca == null || !jogosBiblioteca.Items.Any())
            {
                _logger.LogInformation("Consulta finalizada. Nenhum jogo encontrado na biblioteca para o UserId: {UserId}", currentUserId);
                return NotFound("Não há jogos na biblioteca do usuário.");
            }
            _logger.LogInformation("Consulta de biblioteca bem-sucedida. UserId: {UserId}, Quantidade de Jogos Retornados: {QuantidadeJogos}", currentUserId, jogosBiblioteca.Items.Count());
            return Ok(jogosBiblioteca);
        }
        
    }
}
