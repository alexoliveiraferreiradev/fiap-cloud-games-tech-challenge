using FiapCloudGames.Application.Services.Interfaces;

namespace FiapCloudGames.Infrastructure.Security
{
    public class PasswordHasher : IPasswordHasherService
    {
        public string HashPassword(string password) { return BCrypt.Net.BCrypt.HashPassword(password); }

        public bool VerifyPassword(string password, string hashedPassoword) { return BCrypt.Net.BCrypt.Verify(password, hashedPassoword); } 
    }
}
