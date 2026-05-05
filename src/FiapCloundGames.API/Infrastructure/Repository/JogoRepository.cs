using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance;
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

        public async Task<int> TotalJogosPromovidos()
        {
            var agora = DateTime.UtcNow;
            return await _dbContext.Jogos
                .Where(x => x.Promocoes.Any(p => p.Ativo && p.Periodo.DataInicio <= agora && p.Periodo.DataFim >= agora))
                .Include(x => x.Promocoes.Where(p => p.Ativo && p.Periodo.DataInicio <= agora && p.Periodo.DataFim >= agora))
                .AsNoTracking() 
                .CountAsync();
        }
        public async Task<IEnumerable<Jogo>> ObtemJogosPromovidosPaginacao(int pagina = 1, int tamanhoPagina = 10)
        {
            var agora = DateTime.UtcNow;
            return await _dbContext.Jogos
                .Where(x => x.Promocoes.Any(p => p.Ativo && p.Periodo.DataInicio <= agora && p.Periodo.DataFim >= agora))
                .Include(x => x.Promocoes.Where(p => p.Ativo && p.Periodo.DataInicio <= agora && p.Periodo.DataFim >= agora))
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task DesativaPromocoesInvalidas()
        {
            var agora = DateTime.UtcNow;
            await _dbContext.Jogos
                .SelectMany(j => j.Promocoes)
                .Where(p => p.Ativo && p.Periodo.DataFim <= agora)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Ativo, false).SetProperty(p => p.DataAlteracao, DateTime.UtcNow));
        }

        public async Task<IEnumerable<Jogo>> ObtemPorGeneroPaginado(GeneroJogo generoJogo, int pagina = 1, int tamanhoPagina = 10)
        {
            return await _dbContext.Jogos.
                Where(x => x.Ativo == true && x.Genero == generoJogo)
                .Skip((pagina - 1) * tamanhoPagina)
                .Take(tamanhoPagina)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<int> TotalJogoPorGenero(GeneroJogo generoJogo)
        {
            return await _dbContext.Jogos.
                Where(x => x.Ativo == true && x.Genero == generoJogo)
                .AsNoTracking()
                .CountAsync();
        }

        public async Task<Jogo?> ObtemPorNome(string nomeJogo)
        {
            return await _dbContext.Jogos.FirstOrDefaultAsync(x => x.Nome.Valor.ToLower() == nomeJogo.ToLower());
        }

        public async Task<Promocao?> ObterPromocaoPorId(Guid id)
        {
            var jogosPorPromocao = await _dbContext.Jogos
                .Include(j => j.Promocoes)
                .FirstOrDefaultAsync(j => j.Promocoes.Any(p => p.Id == id));

            return jogosPorPromocao?.Promocoes.FirstOrDefault(p => p.Id == id);
        }

        public async Task<IEnumerable<Jogo>> ObterJogosPorIds(IEnumerable<Guid> jogosIds)
        {
            return await _dbContext.Jogos.Where(x => jogosIds.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Jogo>> ObtemCatalogoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
           return await _dbContext.Jogos.AsNoTracking() 
                        .Where(j => j.Ativo)
                        .OrderBy(j => j.Nome.Valor) 
                        .Skip((pagina - 1) * tamanhoPagina)
                        .Take(tamanhoPagina)
                        .AsNoTracking()
                        .ToListAsync();
        }
    }
}
