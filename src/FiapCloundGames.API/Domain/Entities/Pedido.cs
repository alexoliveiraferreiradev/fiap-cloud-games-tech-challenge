using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Pedido : AggregateRoot
    {
        public Guid UsuarioId { get; private set; }
        public PedidoStatus Status { get; private set; }
        public decimal ValorTotal { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set;  }
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
            DataCadastro = DateTime.UtcNow;
            DataAlteracao = DataCadastro;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            if (UsuarioId == Guid.Empty) throw new DomainException(MensagensDominio.PedidoSemUsuario);
        }

        public void AdicionarItem(Guid jogoId, Preco preco)
        {
            if (Status != PedidoStatus.Rascunho) throw new DomainException(MensagensDominio.PedidoJogoNaoRascunhos);
            if (jogoId == Guid.Empty) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            if (_jogos.Any(j => j.JogoId == jogoId)) throw new DomainException(MensagensDominio.PedidoJogoJaAdicionado);

            _jogos.Add(new PedidoJogo(jogoId, preco));
        }

        public void FinalizarPedido()
        {
            if (Status != PedidoStatus.Rascunho) throw new DomainException(MensagensDominio.PedidoNaoRascunhos);
            if (!_jogos.Any()) throw new DomainException(MensagensDominio.PedidoSemJogos);
            Status = PedidoStatus.Finalizado;
            DataAlteracao = DateTime.UtcNow;
        }

        private void CalcularValorTotal()
        {
            ValorTotal = _jogos.Sum(j => j.PrecoNoMomento.Valor);
        }
    }
}
