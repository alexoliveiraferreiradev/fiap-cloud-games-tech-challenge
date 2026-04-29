using FiapCloundGames.API.Domain.Common.Interfaces;

namespace FiapCloundGames.API.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            throw new NotImplementedException();
        }

        public bool Verify(string password, string hashedPassoword)
        {
            throw new NotImplementedException();
        }
    }
}
