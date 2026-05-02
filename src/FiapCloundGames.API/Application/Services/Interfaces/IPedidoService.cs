using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<Pedido> RealizarPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<Pedido> ObterPedidoPorId(Guid id);
        Task<IEnumerable<Pedido>> ObtemHistoricoPorUsuario(Guid usuarioId);
    }
}
