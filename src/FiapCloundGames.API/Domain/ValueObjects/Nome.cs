using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Nome : ValueObject
    {
        public string Valor { get; }
        public Nome(string nomeUsuario)
        {
            AssertionConcern.AssertArgumentEmpty(nomeUsuario, MensagensDominio.UsuarioNomeObrigatorio);
            AssertionConcern.AssertArgumentLength(nomeUsuario, 3, 20, MensagensDominio.UsuarioTamanhoNomeInvalido);
            Valor = nomeUsuario;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Valor;
        }
    }
}
