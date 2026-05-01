using FiapCloundGames.API.Domain.Common;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task Adicionar(T entity);   
        Task Atualizar(T entity);   
        Task<T> ObterPorId(Guid id);   
        Task<IEnumerable<T>> ObterTodos();  
    }
}
