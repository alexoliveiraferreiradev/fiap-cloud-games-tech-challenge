using FiapCloundGames.API.Application.Dtos.Identity;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Enum;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FiapCloundGames.API.Infrastructure.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly ILogger<JwtTokenService> _logger;
        private readonly TokenSettings _jwtSettings;
        public JwtTokenService(IOptions<TokenSettings> jwtSettings,
            ILogger<JwtTokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _logger = logger;
        }
        public async Task<TokenResult> GerarToken(UsuarioResponse usuario)
        {
            _logger.LogInformation("Iniciando geração de token. ID: {id}", usuario.Id);
            var claims = await ObtemClaims(usuario);
            var acessToken = ObtemToken(claims);
            _logger.LogInformation("Token gerado com sucesso. ID: {id}", usuario.Id);
            return new TokenResult
            {
                AccessToken = acessToken,
                ExpiresIn = _jwtSettings.ExpiracaoHoras *3600,
                Claims = claims.Select(c=>new ClaimResponse { Type = c.Type,Value = c.Value })
            };
        }

        public string ObtemToken(IEnumerable<Claim> claims)
        {
            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtSettings.Emissor,
                Audience = _jwtSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(tokenDescriptor);
        }

        public async Task<IEnumerable<Claim>> ObtemClaims(UsuarioResponse usuario)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Email, usuario.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));
            if (usuario.PerfilUsuario == TipoUsuario.Administrador) claims.Add(new Claim(ClaimTypes.Role, "AdminRole"));
            if (usuario.PerfilUsuario == TipoUsuario.Jogador) claims.Add(new Claim(ClaimTypes.Role, "JogadorRole"));
            return claims;
        }



        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
