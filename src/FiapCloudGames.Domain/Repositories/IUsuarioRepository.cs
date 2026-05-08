
using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmail(string email);
        Task<bool> VerificaEmailCadastrado(string emailCadastrado);
        Task<bool> VerificaMaisDeUmAdminCadastrado();
        Task<bool> VerificaNomeCadastrado(string nomeCadastrado);
        Task<bool> VerificaNomeCadastradoParaAlteracao(Guid usuarioId,string nomeCadastrado);
    }
}
