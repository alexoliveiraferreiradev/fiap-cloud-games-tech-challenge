using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IBibliotecaRepository : IRepository<Biblioteca>
    {
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<IEnumerable<BibliotecaResponse>> ObterJogosPorUsuario(Guid usuarioId);
    }
}
