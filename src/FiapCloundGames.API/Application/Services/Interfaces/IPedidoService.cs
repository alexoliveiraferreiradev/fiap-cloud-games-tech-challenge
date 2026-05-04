using FiapCloundGames.API.Application.Dtos.Pedido;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IPedidoService
    {
        Task<PedidoResponse> RealizarPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<PedidoResponse> ObterPedidoPorId(Guid id);
        Task<IEnumerable<PedidoResponse>> ObtemHistoricoPorUsuario(Guid usuarioId);
    }
}
