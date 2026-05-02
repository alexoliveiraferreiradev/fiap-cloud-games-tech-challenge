using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class PedidoJogo : EntityBase   
    {
        public virtual Jogo Jogo { get;private set;  }
        public Guid JogoId { get; private set; }
        public Preco ValorUnitario { get; private set; }

        protected PedidoJogo()
        {
        }

        public PedidoJogo(Guid jogoId, Preco valorUnitario)
        {
            JogoId = jogoId;
            ValorUnitario = valorUnitario;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotNull(JogoId, MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentNotNull(ValorUnitario, MensagensDominio.PrecoObrigatorio);
        }
    }
}
