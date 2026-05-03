using System.Security.Claims;

namespace FiapCloundGames.API.Domain.Common.Interfaces
{
    public interface IToken
    {
        Task<string> GenerateToken(string configurationTagName, string emailUsuario, IEnumerable<Claim> claims);

        bool ValidateToken(string token);   

        string TokenGenerate(IEnumerable<Claim> claims, string configurationTagName);
    }
}
