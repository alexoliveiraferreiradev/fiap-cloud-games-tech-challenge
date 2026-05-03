using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Senha : ValueObject<Senha>
    {
        public string Hash { get; }

        public Senha(string hash)
        {
            AssertionConcern.AssertArgumentEmpty(hash, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentLength(hash, 8, 60, MensagensDominio.SenhaTamanhoInvalido);
            AssertionConcern.AssertArgumentPasswordStrenght(hash, MensagensDominio.UsuarioSenhaFraca);
            Hash = hash;
        }

        protected override bool EqualsCore(Senha other)
        {
            return Hash == other.Hash;    
        }

        protected override int GetHashCodeCore()
        {
            return Hash.GetHashCode();
        }
    }
}
