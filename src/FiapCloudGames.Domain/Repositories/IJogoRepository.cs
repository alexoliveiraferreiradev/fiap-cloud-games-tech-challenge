using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enum;

namespace FiapCloudGames.Domain.Repositories
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        Task<Jogo?> ObtemPorNome(string nomeJogo);
        Task<IEnumerable<Jogo>> ObtemJogosAtivos();           
        Task<Promocao?> ObterPromocaoPorId(Guid id);       
        Task<IEnumerable<Jogo>> ObterJogosPorIds(IEnumerable<Guid> jogosIds);
        Task DesativaPromocoesInvalidas();
        Task<IEnumerable<Jogo>> ObtemCatalogoPaginado(int pagina = 1, int tamanhoPagina = 10);
        Task<PagedResult<Jogo>> ObtemPaginado(int pagina = 1, int tamanho = 10, string? termoBusca = "", 
            GeneroJogo? generoJogo = null, bool? promocao = false);
    }
}
