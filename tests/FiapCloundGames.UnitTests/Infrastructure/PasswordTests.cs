using FiapCloundGames.API.Infrastructure.Security;

namespace FiapCloundGames.UnitTests.Infrastructure
{
    public class PasswordTests
    {
        [Fact(DisplayName = "Deve gerar um hash BCrypt válido")]
        [Trait("Categoria","Password Tests")]
        public void Hash_DeveGerarStringCriptografada()
        {
            // Arrange - Classe REAL, sem Mock
            var hasher = new PasswordHasher();
            var senhaPura = "123456";

            // Act
            var hash = hasher.HashPassword(senhaPura);

            // Assert
            Assert.StartsWith("$2", hash); // Hashes BCrypt começam com $2a, $2b ou $2y
            Assert.True(BCrypt.Net.BCrypt.Verify(senhaPura, hash));
        }
    }
}
