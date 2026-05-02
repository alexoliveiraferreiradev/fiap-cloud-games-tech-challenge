using FiapCloundGames.API.Infrastructure.Security;

namespace FiapCloundGames.UnitTests.Infrastructure
{
    public class PasswordTests
    {
        [Fact(DisplayName = "Deve gerar um hash BCrypt válido")]
        [Trait("Categoria","Password Tests")]
        public void Hash_DeveGerarStringCriptografada()
        {
            // Arrange 
            var hasher = new PasswordHasher();
            var senhaPura = "123456";
            // Act
            var hash = hasher.HashPassword(senhaPura);
            // Assert
            Assert.StartsWith("$2", hash); 
            Assert.True(BCrypt.Net.BCrypt.Verify(senhaPura, hash));
        }

        [Fact(DisplayName = "Deve verificar um hash válido")]
        [Trait("Categoria","Password Tests")]
        public void Hash_DeveVerificarPassword()
        {
            // Arrange - Classe REAL, sem Mock
            var hasher = new PasswordHasher();
            var senhaPura = "MinhaSenha@123";
            var hashGerado = hasher.HashPassword(senhaPura);    
            // Act
            var result = hasher.VerifyPassword(senhaPura, hashGerado);
            // Assert
            Assert.True(result, "O VerifyPassword deveria ter validado a senha correta.");
        }
    }
}
