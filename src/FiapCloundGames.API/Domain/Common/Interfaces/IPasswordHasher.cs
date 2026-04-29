namespace FiapCloundGames.API.Domain.Common.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);   
        bool Verify(string password, string hashedPassoword);

    }
}
