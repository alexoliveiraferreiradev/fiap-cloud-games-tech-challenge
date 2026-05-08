using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enum;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
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
     
        public async Task DesativaPromocoesInvalidas()
        {
            var agora = DateTime.UtcNow;
            await _dbContext.Jogos
                .SelectMany(j => j.Promocoes)
                .Where(p => p.Ativo && p.Periodo.DataFim <= agora)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Ativo, false).SetProperty(p => p.DataAlteracao, DateTime.UtcNow));
        }    

        public async Task<Jogo?> ObtemPorNome(string nomeJogo)
        {
            return await _dbContext.Jogos.FirstOrDefaultAsync(x => x.Nome.Valor.ToLower() == nomeJogo.ToLower());
        }

        public async Task<Promocao?> ObterPromocaoPorId(Guid id)
        {
            var jogo = await _dbContext.Jogos.Include(j => j.Promocoes)
                .FirstOrDefaultAsync(j => j.Promocoes.Any(p => p.Id == id)) ;

            return jogo?.Promocoes.FirstOrDefault(p => p.Id == id);
        }


        public async Task<IEnumerable<Jogo>> ObterJogosPorIds(IEnumerable<Guid> jogosIds)
        {
            return await _dbContext.Jogos.Where(x => jogosIds.Contains(x.Id)).ToListAsync();
        }


        public async Task<PagedResult<Jogo>> ObtemPaginado(int pagina = 1, int tamanho = 10, string? termoBusca = "", 
            GeneroJogo? generoJogo = null, bool? promocao = false)
        {
            var query = _dbContext.Jogos.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(termoBusca))
                query = query.Where(j => j.Nome.Valor.Contains(termoBusca));

            if (generoJogo.HasValue)
                query = query.Where(j => j.Genero == generoJogo.Value);

            if (promocao == true)
                query = query.Include(j => j.Promocoes.Where(p => p.Ativo))
                     .Where(x => x.Promocoes.Any(p => p.Ativo));

            var totalItens = await query.CountAsync();

            var itens = await query
             .OrderBy(j => j.Nome.Valor)
            .Skip((pagina - 1) * tamanho)
            .Take(tamanho)
            .ToListAsync();

            return new PagedResult<Jogo>(itens, pagina,tamanho,totalItens);
        }
    }
}
