using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
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
                 .Where(x => x.UsuarioId == usuarioId)
                 .OrderByDescending(x=>x.DataCadastro)
                 .ToListAsync();
        }
    }
}
