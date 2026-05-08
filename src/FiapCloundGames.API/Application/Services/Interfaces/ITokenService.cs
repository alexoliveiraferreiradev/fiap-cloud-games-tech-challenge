using FiapCloundGames.API.Application.Dtos.Identity;
using FiapCloundGames.API.Application.Dtos.Usuario;
using System.Security.Claims;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface ITokenService
    {
        string ObtemToken(IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> ObtemClaims(UsuarioResponse usuario);
        Task<TokenResult> GerarToken(UsuarioResponse usuario);
    }
}
