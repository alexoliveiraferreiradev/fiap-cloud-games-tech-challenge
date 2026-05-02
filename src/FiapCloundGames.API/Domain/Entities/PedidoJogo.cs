using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class PedidoJogo
    {
        public Guid JogoId { get; private set; }
        public Preco PrecoNoMomento { get; private set; }

        protected PedidoJogo()
        {
        }

        public PedidoJogo(Guid jogoId, Preco precoNoMomento)
        {
            ValidaEntidades(jogoId, precoNoMomento);
            JogoId = jogoId;
            PrecoNoMomento = precoNoMomento;
        }

        private void ValidaEntidades(Guid jogoId,Preco preco)
        {
            AssertionConcern.AssertArgumentNotNull(jogoId, MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentNotNull(preco, MensagensDominio.PrecoObrigatorio);
        }
    }
}
