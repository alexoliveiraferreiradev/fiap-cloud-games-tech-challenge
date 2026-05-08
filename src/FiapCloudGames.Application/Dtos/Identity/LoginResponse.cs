using FiapCloudGames.Application.Dtos.Usuario;
using FiapCloudGames.Domain.Enum;

namespace FiapCloudGames.Application.Dtos.Identity
{
    public class LoginResponse
    {
        public string AcessToken { get; set; }
        public double ExpiresIn { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimResponse> Claims { get; set; }
        public TipoUsuario PerfilUsuario { get; set; }
    }
}
