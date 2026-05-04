using Azure;
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
            _logger.LogInformation("Iniciando processo de autenticação. Email: {Email}", loginRequest.Email);
            var usuarioResponse = await _usuarioService.Autenticar(loginRequest);
            if (usuarioResponse == null)
            {
                _logger.LogInformation("Usuário não foi encontrado no banco de dados. EMAIL: {email}", loginRequest.Email);
                return NotFound("Usuário não encotrado.");
            }
            var response = await _tokenConfiguration.RetornaJwt(usuarioResponse);
            _logger.LogInformation("Login realizado com sucesso. UsuarioId: {UserId}, Email: {Email}", usuarioResponse.Id, loginRequest.Email);
            return StatusCode(StatusCodes.Status200OK,response);
        }

        /// <summary>
        /// Registra um novo usuário no sistema e retorna o token de acesso.
        /// </summary>
        /// <remarks>
        /// Este endpoint realiza a criação da conta e, em caso de sucesso, 
        /// já autentica o usuário automaticamente, retornando o token JWT.
        /// 
        /// Possíveis mensagens de erro (Bad Request):
        /// 
        /// * **Validação de Nome:** Obrigatório, entre 3 e 20 caracteres.
        /// * **Validação de E-mail:** Obrigatório, formato válido, verificação de duplicidade.
        /// * **Validação de Senha:** Mínimo 8 caracteres (A-z, 0-9, @#$), confirmação idêntica.
        /// 
        /// Exemplo de requisição:
        /// 
        ///     POST /cadastrar
        ///     {
        ///        "nome": "Nome usuario",
        ///        "email": "usuario.dev@exemplo.com",
        ///        "senha": "SenhaForte@2026"
        ///     }
        ///     Exemplo de erro:
        /// 
        ///     {
        ///        "errors":[
        ///         "O nome do usuário é obrigatório.",
        ///         "O nome do usuário deve conter entre 3 e 20 caracteres.",
        ///         "O email do usuário é obrigatório.",
        ///         "O email é inválido.",
        ///         "O tamanho do email é inválido.",
        ///         "O email já foi cadastrado.",
        ///         "O nome de usuário já foi cadastrado.",
        ///         "A senha e a confirmação de senha devem ser iguais.",
        ///         "A senha do usuário é obrigatória.",
        ///         "O tamanho da senha é inválido.",
        ///         "A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais."
        ///        ]
        ///     }
        /// 
        /// </remarks>
        /// <param name="request">Objeto contendo os dados necessários para o cadastro (Nome, E-mail e Senha).</param>
        /// <returns>Retorna os dados do perfil criado e o token de autenticação.</returns>
        /// <response code="201">Usuário criado com sucesso e autenticado.</response>
        /// <response code="400">Dados inválidos ou e-mail já cadastrado no sistema.</response>
        [HttpPost("cadastrar")]
        [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Cadastrar(CriaUsuarioRequest request)
        {
            _logger.LogInformation("Iniciando tentativa de cadastro de novo usuário. Email: {Email}", request.Email);
            var usuario = await _usuarioService.CadastrarUsuario(request);
            _logger.LogInformation("Usuário criado com sucesso. Gerando token de acesso. UserId: {UserId}, Email: {Email}", usuario.Id, request.Email);
            var response = await _tokenConfiguration.RetornaJwt(usuario);
            _logger.LogInformation("Fluxo de cadastro e autenticação inicial concluído. UserId: {UserId}", usuario.Id);
            return StatusCode(StatusCodes.Status201Created, response);
        }

        
    }
}
