using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<AutenticacaoController> _logger;
        private readonly IToken _tokenConfiguration;
        public AutenticacaoController(IUsuarioService usuarioService, 
            ILogger<AutenticacaoController> logger,
            IToken tokeConfiguration)
        {
            _usuarioService = usuarioService;
            _logger = logger;
            _tokenConfiguration = tokeConfiguration;
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            var usuario = await _usuarioService.Autenticar(loginRequest);
            if (usuario == null)
            {
                _logger.LogInformation("Usuário não foi encontrado no banco de dados. EMAIL: {email}", loginRequest.Email);
                return NotFound("Usuário não encotrado.");
            }
            return await _tokenConfiguration.RetornaJwt(usuario);
        }      
    }
}
