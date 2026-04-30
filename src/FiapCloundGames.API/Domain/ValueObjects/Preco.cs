using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Preco : ValueObject
    {
        public decimal Valor { get; }

        public Preco(decimal valor)
        {
            AssertionConcern.AssertArgumentValueFormat(valor, MensagensDominio.ValorInvalido);
            Valor = valor;  
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }
    }
}
