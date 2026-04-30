using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class PedidoJogo
    {
        public Guid JogoId { get; set; }
        public decimal PrecoNoMomento { get; set; }

        protected PedidoJogo()
        {
        }

        public PedidoJogo(Guid jogoId, decimal precoNoMomento)
        {
            ValidaEntidades(jogoId, precoNoMomento);
            JogoId = jogoId;
            PrecoNoMomento = precoNoMomento;
        }

        private void ValidaEntidades(Guid jogoId, decimal precoNoMomento)
        {
            AssertionConcern.AssertArgumentNotNull(jogoId, MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentValueFormat(precoNoMomento, MensagensDominio.JogoPrecoInvalido);
        }
    }
}
