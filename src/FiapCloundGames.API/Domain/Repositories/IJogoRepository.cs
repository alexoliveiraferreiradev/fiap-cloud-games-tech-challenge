using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        Task<Jogo?> ObtemPorNome(string nomeJogo);
        Task<IEnumerable<Jogo>> ObtemJogosAtivos();
        Task<Jogo?> ObtemJogoAtivo(Guid id);
        Task<int> TotalJogosPromovidos();        
        Task<IEnumerable<Jogo>> ObtemJogosPromovidosPaginacao(int pagina =1, int tamanhoPagina = 10);        
        Task<Promocao?> ObterPromocaoPorId(Guid id);
        Task<IEnumerable<Jogo>> ObtemPorGeneroPaginado(GeneroJogo generoJogo, int pagina =1, int tamanhoPagina = 10);
        Task<int> TotalJogoPorGenero(GeneroJogo generoJogo);

        Task<IEnumerable<Jogo>> ObterJogosPorIds(IEnumerable<Guid> jogosIds);
        Task DesativaPromocoesInvalidas();
        Task<IEnumerable<Jogo>> ObtemCatalogoPaginado(int pagina = 1, int tamanhoPagina = 10);
    }
}
