using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Senha : ValueObject<Senha>
    {
        public string Hash { get; }

        public Senha(string senha)
        {
            AssertionConcern.AssertArgumentLength(3,8,MensagensDominio.senhatam)
            AssertionConcern.AssertArgumentEmpty(senha, MensagensDominio.UsuarioSenhaObrigatoria);
            AssertionConcern.AssertArgumentPasswordStrenght(senha, MensagensDominio.UsuarioSenhaFraca);
            Hash = senha;
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
