using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
{
    public class PedidoRepository : Repository<Pedido>, IPedidoRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PedidoRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<IEnumerable<Pedido>> ObtemHistoricoPorUsuario(Guid usuarioId)
        {
            return await _dbContext.Pedidos
                 .AsNoTracking()
                 .Include(p=>p.Jogos)
                 .ThenInclude(pj=>pj.Jogo)
                 .Where(x => x.UsuarioId == usuarioId)
                 .OrderByDescending(x=>x.DataCadastro)
                 .ToListAsync();
        }
    }
}
