using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Task<Usuario> PromoverParaAdmin(Guid id);
        Task<Usuario> RebaixarParaJogador(Guid id,Guid idOperador);
        Task<Usuario> CadastrarUsuario(CriaUsuarioRequest request);
        Task<Usuario> AtualizarUsuario(Guid usuarioId, UpdateUsuarioRequest updateUsuarioRequest);
        Task Desativar(DesativaUsuarioRequest deleteUsuarioRequest,Guid idOperador);
        Task DesativarConta(Guid id);
        Task Reativar(Guid usuarioId);
        Task<Usuario> Autenticar(LoginRequest loginRequest);
        Task<Usuario> ObterPorId(Guid usuarioId);
        Task<IEnumerable<Usuario>> ObterTodos();
        Task<Usuario?> ObterPorEmail(string emailUsuario);
        Task<bool> VerificaAdminCadastrado();

    }
}