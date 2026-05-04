using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using System.Security.Claims;

namespace FiapCloundGames.API.Domain.Common.Interfaces
{
    public interface IToken
    {
        string TokenGenerate(IEnumerable<Claim> claims);
        Task<IEnumerable<Claim>> ObtemClaims(UsuarioResponse usuario);
        Task<LoginResponse> RetornaJwt(UsuarioResponse usuario);
    }
}
