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

        /// <summary>
        /// Autentica um usuário e gera um token de acesso.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        /// 
        ///     POST /login
        ///     {
        ///        "email": "usuario@exemplo.com",
        ///        "senha": "SenhaForte123!"
        ///     }
        /// 
        /// </remarks>
        /// <param name="loginRequest">Objeto contendo as credenciais de acesso (E-mail e Senha).</param>
        /// <returns>Retorna os dados do perfil do usuário e o Token JWT gerado.</returns>
        /// <response code="200">Retorna o token de acesso e os dados básicos do usuário.</response>
        /// <response code="400">Dados de entrada inválidos (ex: formato de e-mail incorreto).</response>
        /// <response code="401">Credenciais inválidas (e-mail ou senha incorretos).</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        
        [HttpPost("cadastrar")]
        public async Task<ActionResult<LoginResponse>> Cadastrar(CriaUsuarioRequest request)
        {
            var usuario = await _usuarioService.CadastrarUsuario(request);          
            return await _tokenConfiguration.RetornaJwt(usuario);
        }

        
    }
}
