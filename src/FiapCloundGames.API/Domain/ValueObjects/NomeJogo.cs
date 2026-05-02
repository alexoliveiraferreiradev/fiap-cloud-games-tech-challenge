using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class NomeJogo : ValueObject<NomeJogo>
    {
        public string Valor { get; }

        public NomeJogo(string valor)
        {
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
