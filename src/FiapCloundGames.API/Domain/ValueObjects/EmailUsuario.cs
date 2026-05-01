using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class EmailUsuario : ValueObject
    {
        public string Email { get;  }

        public EmailUsuario(string emailUsuario)
        {
            AssertionConcern.AssertArgumentEmpty(emailUsuario, MensagensDominio.UsuarioEmailObrigatorio);
            AssertionConcern.AssertArgumentEmailFormat(emailUsuario, MensagensDominio.EmailInvalido);
            Email = emailUsuario;   
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Email;
        }
    }
}
