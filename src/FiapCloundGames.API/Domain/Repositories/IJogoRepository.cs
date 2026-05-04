using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IJogoRepository : IRepository<Jogo>
    {
        Task<Jogo?> ObtemPorNome(string nomeJogo);
        Task<IEnumerable<Jogo>> ObtemJogosAtivos();
        Task<IEnumerable<Jogo>> ObtemJogosPromovidos();
        Task<Promocao?> ObterPromocaoPorId(Guid id);
        Task<IEnumerable<Jogo>> ObtemPorGenero(GeneroJogo generoJogo);
        Task<IEnumerable<Jogo>> ObterJogosPorIds(IEnumerable<Guid> jogosIds);
        Task DesativaPromocoesInvalidas();
    }
}
