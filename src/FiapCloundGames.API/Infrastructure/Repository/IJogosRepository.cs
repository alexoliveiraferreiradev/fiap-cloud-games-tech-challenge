using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public interface IJogosRepository : IRepository<Jogos>
    {
        Task<Jogos> ObtemPorNome(string nomeJogo);
        
    }
}
