using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Resources;

namespace FiapCloudGames.Domain.ValueObjects
{
    public class Preco : ValueObject<Preco>
    {
        public decimal Valor { get; }

        public Preco(decimal valor)
        {
            AssertionConcern.AssertArgumentValueFormat(valor, MensagensDominio.ValorInvalido);
            Valor = valor;  
        }

        protected override bool EqualsCore(Preco other)
        {
           return Valor == other.Valor; 
        }

        protected override int GetHashCodeCore()
        {
           return Valor.GetHashCode();
        }
    }
}
