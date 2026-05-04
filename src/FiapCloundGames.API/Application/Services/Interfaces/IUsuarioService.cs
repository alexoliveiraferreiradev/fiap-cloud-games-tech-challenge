using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Task<UsuarioResponse> PromoverParaAdmin(Guid id);
        Task<UsuarioResponse> RebaixarParaJogador(Guid id,Guid idOperador);
        Task<LoginResponse> CadastrarUsuario(CriaUsuarioRequest request);
        Task<LoginResponse> AtualizarUsuario(Guid usuarioId, UpdateUsuarioRequest updateUsuarioRequest);
        Task Desativar(DesativaUsuarioRequest deleteUsuarioRequest,Guid idOperador);
        Task DesativarConta(Guid id);
        Task Reativar(Guid usuarioId);
        Task<LoginResponse> Autenticar(LoginRequest loginRequest);
        Task<UsuarioResponse> ObterPorId(Guid usuarioId);
        Task<IEnumerable<UsuarioResponse>> ObterTodos();
        Task<UsuarioResponse?> ObterPorEmail(string emailUsuario);
        Task<bool> VerificaAdminCadastrado();

    }
}