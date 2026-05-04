using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Pedido
{
    public class PedidoResponse
    {
        public Guid Id { get; set; }
        public Guid JogoId { get; set; }
        public DateTime DataPedido { get; set; }
        public PedidoStatus Status { get; set; }       
        public decimal ValorTotal { get; set; }
        public List<string> MensagensInformativas { get; set; } = new();
        public IEnumerable<PedidoItemResponse> Items { get; set; }
        public PedidoResponse()
        {
            
        }
    }

    public class PedidoItemResponse
    {
        public string NomeJogo { get; set; }
        public decimal PrecoOriginal { get; set; }
        public decimal Desconto { get; set; }
        public decimal PrecoPago { get; set; }

        public PedidoItemResponse()
        {
            Desconto = PrecoOriginal - PrecoPago;
        }
    }
}
