using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
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

        public async Task<PagedResult<Biblioteca>> ObterJogosPorUsuarioPaginacao(Guid usuarioId, int pagina = 1, int tamanhoPagina = 10)
        {
            var query = _dbContext.Bibliotecas.Where(x => x.UsuarioId == usuarioId).AsNoTracking().AsQueryable();
            var totalItens = await query.CountAsync();
            var itensUsuario = await query
                            .Include(j=>j.Jogo)
                            .OrderBy(b => b.DataCadastro)
                            .Skip((pagina - 1) * tamanhoPagina)
                            .Take(tamanhoPagina)
                            .ToListAsync();

            return new PagedResult<Biblioteca>(itensUsuario, pagina, tamanhoPagina, totalItens);
        }

        public async Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId)
        {
            return await _dbContext.Bibliotecas.AnyAsync(p => p.UsuarioId == usuarioId && p.JogoId == jogoId);
        }
    }
}
