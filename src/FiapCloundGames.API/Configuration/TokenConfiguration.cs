using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Configuration.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FiapCloundGames.API.Configuration
{
    public class TokenConfiguration : IToken
    {
        private readonly IConfiguration _configuration;
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<TokenConfiguration> _logger;
        private static string _secreteKey = string.Empty;
        private static string _issuer = string.Empty;
        private static string _audience = string.Empty;
        private static string _expirationHours = string.Empty;
        public TokenConfiguration(IConfiguration configuration, IUsuarioService usuarioService,
            ILogger<TokenConfiguration> logger)
        {
            _configuration = configuration;
            _usuarioService = usuarioService;
            _logger = logger;   
        }

        
        public async Task<string> GenerateToken(string configurationTagName, string emailUsuario, IEnumerable<Claim> claims)
        {
            _logger.LogInformation("Requisição iniciado para gerar token de usuário. EMAIL: {email}", emailUsuario);
            var usuario = await _usuarioService.ObterPorEmail(emailUsuario);
            if (usuario == null)
            {
                _logger.LogInformation("Não foi encontrado nenhum usuário com este email. EMAIL: {email}", emailUsuario);
                throw new BusinessException("Não foi encontrado nenhum usuario");                
            }
            var token = TokenGenerate(claims, configurationTagName);
            return token;
        }


        public bool ValidateToken(string token)
        {
            throw new NotImplementedException();
        }

        public string TokenGenerate(IEnumerable<Claim> claims, string configurationTagName)
        {
            _secreteKey = _configuration[$"{configurationTagName}:Secret"] ?? string.Empty;
            _issuer = _configuration[$"{configurationTagName}:Emissor"] ?? string.Empty;
            _audience = _configuration[$"{configurationTagName}:ValidoEm"] ?? string.Empty;
            _expirationHours = _configuration[$"{configurationTagName}:ExpiracaoHoras"];
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secreteKey);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _issuer,
                Audience = _audience,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(double.Parse(_expirationHours ?? "1")),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }
    }
}
