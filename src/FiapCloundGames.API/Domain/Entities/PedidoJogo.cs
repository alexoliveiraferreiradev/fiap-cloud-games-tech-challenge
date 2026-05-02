using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class PedidoJogo : EntityBase   
    {
        public Guid JogoId { get; private set; }
        public Preco PrecoNoMomento { get; private set; }

        protected PedidoJogo()
        {
        }

        public PedidoJogo(Guid jogoId, Preco precoNoMomento)
        {
            JogoId = jogoId;
            PrecoNoMomento = precoNoMomento;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotNull(JogoId, MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentNotNull(PrecoNoMomento, MensagensDominio.PrecoObrigatorio);
        }
    }
}
