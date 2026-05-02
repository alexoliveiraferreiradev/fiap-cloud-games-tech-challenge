using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Nome : ValueObject<Nome>
    {
        public string Valor { get; }
        public Nome(string nomeUsuario)
        {
            AssertionConcern.AssertArgumentEmpty(nomeUsuario, MensagensDominio.UsuarioNomeObrigatorio);
            AssertionConcern.AssertArgumentLength(nomeUsuario, 3, 50, MensagensDominio.UsuarioTamanhoNomeInvalido);
            Valor = nomeUsuario;
        }

        protected override bool EqualsCore(Nome other)
        {
            return Valor == other.Valor;
        }

        protected override int GetHashCodeCore()
        {
            return Valor.GetHashCode();
        }
    }
}
