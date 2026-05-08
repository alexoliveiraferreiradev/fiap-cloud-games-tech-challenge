using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Resources;

namespace FiapCloudGames.Domain.ValueObjects
{
    public class NomeJogo : ValueObject<NomeJogo>
    {
        public string Valor { get; }

        public NomeJogo(string valor)
        {
            AssertionConcern.AssertArgumentRealValues(valor, MensagensDominio.NomeJogoNaoReal);
            AssertionConcern.AssertArgumentEmpty(valor, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentLength(valor, 3, 100, MensagensDominio.JogoTamanhoNomeInvalido);
            Valor = valor;
        }
        protected override bool EqualsCore(NomeJogo other)
        {
            return Valor == other.Valor;    
        }

        protected override int GetHashCodeCore()
        {
            return Valor.GetHashCode();
        }
    }
}
