using FiapCloudGames.Application.Dtos.Pedido;

namespace FiapCloudGames.Application.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> RealizarPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<PedidoResponse> ObterPedidoPorId(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtemHistoricoPorUsuario(Guid usuarioId);
    }
}
