using FiapCloudGames.Application.Dtos.Identity;
using FiapCloudGames.Application.Dtos.Usuario;
using System.Security.Claims;

namespace FiapCloudGames.Application.Interfaces
{
    public interface ITokenService
    {
        string ObtemToken(IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> ObtemClaims(UsuarioResponse usuario);
        Task<TokenResult> GerarToken(UsuarioResponse usuario);
    }
}
