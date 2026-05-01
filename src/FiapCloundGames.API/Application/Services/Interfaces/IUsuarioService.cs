using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Task<Usuario> CadastrarAdministrador(CriaUsuarioRequest request, bool hasPermision, string token);
        Task<Usuario> CadastrarUsuario(CriaUsuarioRequest request);
        Task AtualizarUsuario(Guid usuarioId, UpdateUsuarioRequest updateUsuarioRequest);
        Task Desativar(DeleteUsuarioRequest deleteUsuarioRequest);
        Task Reativar(Guid usuarioId);
        Task<Usuario> Autenticar(LoginRequest loginRequest);

    }
}