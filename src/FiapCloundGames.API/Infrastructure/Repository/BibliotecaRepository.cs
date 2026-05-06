using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public class BibliotecaRepository : Repository<Biblioteca>, IBibliotecaRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BibliotecaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId)
        {
            return await _dbContext.Bibliotecas.Where(x => x.UsuarioId == usuarioId).Select(x => x.JogoId).ToListAsync();

        }

        public async Task<IEnumerable<Biblioteca>> ObterJogosPorUsuarioPaginacao(Guid usuarioId, int pagina =1, int tamanhoPagina = 10)
        {
            return await _dbContext.Bibliotecas
                     .AsNoTracking() 
                     .Include(b=>b.Jogo)
                    .Where(b => b.UsuarioId == usuarioId)
                     .Skip((pagina - 1) * tamanhoPagina)
                    .Take(tamanhoPagina)
                    .ToListAsync();
        }

        public async Task<int> TotalJogosPorUsuario(Guid usuarioId)
        {
            return await _dbContext.Bibliotecas
                    .AsNoTracking()
                    .Include(b => b.Jogo)
                   .Where(b => b.UsuarioId == usuarioId).CountAsync();
        }

        public async Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId)
        {
            return await _dbContext.Bibliotecas.AnyAsync(p =>p.UsuarioId == usuarioId && p.JogoId == jogoId);
        }
    }
}
