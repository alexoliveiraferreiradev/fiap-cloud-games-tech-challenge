using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Domain.Repositories
{
    public interface IBibliotecaRepository : IRepository<Biblioteca>
    {
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<PagedResult<Biblioteca>> ObterJogosPorUsuarioPaginacao(Guid usuarioId, int pagina =1, int tamanhoPagina = 10);        
        Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId);
    }
}
