using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IJogosRepository : IRepository<Jogo>
    {
        Task<Jogo> ObtemPorNome(string nomeJogo);
        Task<IEnumerable<Jogo>> ObtemJogosAtivos();

        Task<Promocao> ObterPromocaoPorId(Guid id);
    }
}
