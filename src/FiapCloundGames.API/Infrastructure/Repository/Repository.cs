using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public abstract class Repository<Entidade> : IRepository<Entidade> where Entidade : AggregateRoot, new()
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
            await _dbContext.SaveChangesAsync();
        }

        public Task Atualizar(Entidade entity)
        {
            throw new NotImplementedException();
        }

        public Task<Entidade> ObterPorId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entidade>> ObterTodos()
        {
            throw new NotImplementedException();
        }
    }
}
