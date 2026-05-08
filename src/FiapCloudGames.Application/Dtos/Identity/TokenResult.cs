using FiapCloudGames.Application.Dtos.Usuario;

namespace FiapCloudGames.Application.Dtos.Identity
{
    public class TokenResult
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
    }
}
