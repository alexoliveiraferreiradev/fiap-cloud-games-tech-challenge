using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public abstract class Repository<Entidade> : IRepository<Entidade> where Entidade : AggregateRoot
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly DbSet<Entidade> _dbSet;

        protected Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = dbContext.Set<Entidade>();
        }

        public async Task Adicionar(Entidade entity)
        {
            _dbSet.Add(entity);
            await SaveChanges();
        }

        public async Task Atualizar(Entidade entity)
        {
            _dbSet.Update(entity);
            await SaveChanges();
        }

        public async Task<Entidade> ObterPorId(Guid id)
        {
            return await _dbSet.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Entidade>> ObterTodos()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
