using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Pedido : AgreggateRoot
    {
        public Guid UsuarioId { get; private set; }
        public PedidoStatus Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataAdicao { get; private set; }
        private List<PedidoJogo> _jogos;
        public IReadOnlyCollection<PedidoJogo> Jogos => _jogos;

        protected Pedido()
        {
        }

        public Pedido(Guid usuarioId)
        {
            UsuarioId = usuarioId;
            _jogos = new List<PedidoJogo>();
            Status = PedidoStatus.Rascunho;
            DataAdicao = DateTime.UtcNow;
            ValidarEntidade();
        }

        public override void ValidarEntidade()
        {
            if (UsuarioId == Guid.Empty) throw new DomainException(MensagensDominio.PedidoSemUsuario);
        }

        public void AdicionarItem(Guid jogoId, decimal preco)
        {
            if (Status != PedidoStatus.Rascunho) throw new DomainException(MensagensDominio.PedidoJogoNaoRascunhos);
            if (jogoId == Guid.Empty) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            if (preco < 0) throw new DomainException(MensagensDominio.JogoPrecoInvalido);
            if (_jogos.Any(j => j.JogoId == jogoId)) throw new DomainException(MensagensDominio.PedidoJogoJaAdicionado);

            _jogos.Add(new PedidoJogo(jogoId, preco));
        }

        public void FinalizarPedido()
        {
            if (Status != PedidoStatus.Rascunho) throw new DomainException(MensagensDominio.PedidoNaoRascunhos);
            if (_jogos.Any()) throw new DomainException(MensagensDominio.PedidoSemJogos);
            Status = PedidoStatus.Finalizado;
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _jogos.Sum(j => j.PrecoNoMomento);
        }
    }
}
