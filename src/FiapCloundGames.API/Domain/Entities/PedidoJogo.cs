using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class PedidoJogo
    {
        public Guid JogoId { get; set; }
        public string NomeJogo { get; set; }
        public decimal PrecoNoMomento { get; set; }

        protected PedidoJogo()
        {
        }

        public PedidoJogo(Guid jogoId, string nomeJogo, decimal precoNoMomento)
        {
            ValidaEntidades(jogoId, nomeJogo, precoNoMomento);
            JogoId = jogoId;
            NomeJogo = nomeJogo;   
            PrecoNoMomento = precoNoMomento;
        }

        private void ValidaEntidades(Guid jogoId, string nomeJogo, decimal precoNoMomento)
        {
            AssertionConcern.AssertArgumentNotNull(jogoId, MensagensDominio.JogoObrigatório);
            AssertionConcern.AssertArgumentNotEmpty(nomeJogo, MensagensDominio.JogoNomeObrigatorio);  
            AssertionConcern.AssertArgumentValueFormat(precoNoMomento, MensagensDominio.JogoPrecoInvalido);
        }
    }
}
