using FiapCloundGames.API.Application.Dtos.Usuario;

namespace FiapCloundGames.API.Application.Dtos.Identity
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
    }
}
