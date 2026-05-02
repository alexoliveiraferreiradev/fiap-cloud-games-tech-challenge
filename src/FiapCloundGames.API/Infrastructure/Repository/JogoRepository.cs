using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public class JogoRepository : Repository<Jogo>, IJogoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public JogoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {            
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Jogo>> ObtemJogosAtivos()
        {
            return await _dbContext.Jogos.Where(x => x.Ativo == true).ToListAsync();
        }

        public async Task<Jogo?> ObtemPorNome(string nomeJogo)
        {
            return await _dbContext.Jogos.FirstOrDefaultAsync(x => x.Nome.Valor.Equals(nomeJogo));
        }

        public async Task<Promocao?> ObterPromocaoPorId(Guid id)
        {
            var jogosPorPromocao = await _dbContext.Jogos
                .Include(j => j.Promocoes)
                .FirstOrDefaultAsync(j=>j.Promocoes.Any(p=>p.Id == id));

            return jogosPorPromocao?.Promocoes.FirstOrDefault(p => p.Id == id);
        }
    }
}
