using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Domain.Repositories
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Pedido>> ObtemHistoricoPorUsuario(Guid usuarioId);
    }
}
