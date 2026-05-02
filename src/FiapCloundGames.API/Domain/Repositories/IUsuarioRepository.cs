using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> ObterPorEmail(string email);
        Task<bool> VerificaEmailCadastrado(string emailCadastrado);
        Task<bool> VerificaNomeCadastrado(string nomeCadastrado);
    }
}
