using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public record CriaUsuarioRequest(string Nome, string Email, string Senha,string reSenha);
}
