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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BibliotecaResponse>>> ObtemBibliotecaJogador()
        {
            var currentUserIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserIdClaim)) return Unauthorized();

            var currentUserId = Guid.Parse(currentUserIdClaim);
            var jogosBiblioteca = await _bibliotecaService.ObterJogosPorUsuario(currentUserId);
            if (!jogosBiblioteca.Any())
                return NotFound("Não há jogos na biblioteca do usuário.");

            return Ok(jogosBiblioteca);  
        }
        
    }
}
