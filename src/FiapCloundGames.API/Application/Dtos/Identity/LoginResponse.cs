using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Identity
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
