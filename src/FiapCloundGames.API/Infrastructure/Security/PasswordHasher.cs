using FiapCloundGames.API.Domain.Common.Interfaces;

namespace FiapCloundGames.API.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool Verify(string password, string hashedPassoword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassoword);
        }
    }
}
