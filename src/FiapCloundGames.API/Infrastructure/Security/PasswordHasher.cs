using FiapCloundGames.API.Domain.Common.Interfaces;

namespace FiapCloundGames.API.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);

        public bool Verify(string password, string hashedPassoword) => BCrypt.Net.BCrypt.Verify(password, hashedPassoword); 
    }
}
