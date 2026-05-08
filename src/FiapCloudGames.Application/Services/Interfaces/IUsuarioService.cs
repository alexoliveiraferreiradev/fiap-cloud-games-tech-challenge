using FiapCloudGames.Application.Dtos.Identity;
using FiapCloudGames.Application.Dtos.Usuario;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IUsuarioService  
    {
        Task<UsuarioResponse> PromoverParaAdmin(Guid id);
        Task<UsuarioResponse> RebaixarParaJogador(Guid id,Guid idOperador);
        Task<LoginResponse> Cadastrar(CriaUsuarioRequest request);
        Task<UsuarioResponse> Atualizar(Guid usuarioId, UpdateUsuarioRequest updateUsuarioRequest);
        Task Desativar(DesativaUsuarioRequest deleteUsuarioRequest,Guid idOperador);
        Task DesativarConta(Guid id);
        Task Reativar(Guid usuarioId);
        Task<LoginResponse> Autenticar(LoginRequest loginRequest);
        Task<UsuarioResponse> ObterPorId(Guid usuarioId);
        Task<IEnumerable<UsuarioResponse>> ObterTodos();
        Task<UsuarioResponse?> ObterPorEmail(string emailUsuario);

    }
}