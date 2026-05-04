using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Entities;
using System.Data;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IBibliotecaRepository : IRepository<Biblioteca>
    {
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<IEnumerable<Biblioteca>> ObterJogosPorUsuarioPaginacao(Guid usuarioId, int pagina =1, int tamanhoPagina = 10);
        Task<int> TotalJogosPorUsuario(Guid usuarioId);
        Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId);
    }
}
