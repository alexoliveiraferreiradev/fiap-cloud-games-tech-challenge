namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public record UpdateUsuarioRequest(string nomeUsuario, string emailUsuario, string senhaUsuario,string reSenhaUsuario);
}
