using FiapCloundGames.API.Infrastructure.Security;

namespace FiapCloundGames.UnitTests.Infrastructure
{
    public class PasswordTests
    {
        [Fact(DisplayName = "Deve gerar um hash BCrypt válido")]
        public void Hash_DeveGerarStringCriptografada()
        {
            // Arrange
            var hasher = new PasswordHasher();
            var senhaPura = "123456";

            // Act
            var hash = hasher.Hash(senhaPura);

            // Assert
            Assert.StartsWith("$2", hash); 
            Assert.True(BCrypt.Net.BCrypt.Verify(senhaPura, hash));
        }
    }
}
