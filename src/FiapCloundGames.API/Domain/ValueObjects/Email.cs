using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Valor { get;  }

        public Email(string emailUsuario)
        {
            AssertionConcern.AssertArgumentEmpty(emailUsuario, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentEmailFormat(emailUsuario, MensagensDominio.EmailInvalido);
            Valor = emailUsuario;   
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }
    }
}
