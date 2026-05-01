using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.API.Infrastructure.Security;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class SenhaTests
    {
        [Fact(DisplayName = "Valida senha - deve criptografar a senha com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task ValidacaoSenha_DeveCadastrarComSucesso()
        {
            //Arrange
            var senha = "Senha@123";
            //Mock
            var passwordHasher = new PasswordHasher();  
            var repoMock = new Mock<IUsuarioRepository>();
            var hasherMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hasherMock.Object);

            hasherMock.Setup(h => h.HashPassword(senha)).Returns("HashSenha@123");

            //Act
            var result = passwordHasher.HashPassword(senha);
            //Arrange
            Assert.Equal("HashSenha@123", result);
            Assert.NotEqual("Teste@123", result);
        }
    }
}
