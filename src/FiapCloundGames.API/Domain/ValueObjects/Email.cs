using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Email : ValueObject<Email>
    {
        public string Valor { get; }

        public Email(string valor)
        {
            AssertionConcern.AssertArgumentEmpty(valor, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentEmailFormat(valor, MensagensDominio.EmailInvalido);
            AssertionConcern.AssertArgumentLength(valor, 7, 100, MensagensDominio.EmailTamanhpInvalido);
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
