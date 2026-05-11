using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Resources;

namespace FiapCloudGames.Domain.ValueObjects
{
    public class Email : ValueObject<Email>
    {
        public string Valor { get; }

        public Email(string valor)
        {
            AssertionConcern.AssertArgumentRealValues(valor, MensagensDominio.EmailNaoReal);
            AssertionConcern.AssertArgumentEmpty(valor, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentLength(valor, 7, 100, MensagensDominio.EmailTamanhoInvalido);
            AssertionConcern.AssertArgumentEmailFormat(valor, MensagensDominio.EmailInvalido);
            Valor = valor;
        }

        protected override bool EqualsCore(Email other)
        {
            return Valor == other.Valor;
        }

        protected override int GetHashCodeCore()
        {
            return Valor.GetHashCode();
        }
    }
}
