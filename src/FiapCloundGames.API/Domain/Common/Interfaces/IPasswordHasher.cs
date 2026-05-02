namespace FiapCloundGames.API.Domain.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);   
        bool VerifyPassword(string password, string hashedPassoword);

    }
}
