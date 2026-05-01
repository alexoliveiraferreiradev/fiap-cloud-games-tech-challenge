using FiapCloundGames.API.Domain.Common;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Senha : ValueObject<Senha>
    {
        public string Hash { get; }
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
