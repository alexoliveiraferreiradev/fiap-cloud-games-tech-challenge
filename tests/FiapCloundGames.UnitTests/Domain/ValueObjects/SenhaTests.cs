using Bogus;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.API.Infrastructure.Security;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class SenhaTests
    {
        private readonly Faker _faker;
        private readonly UsuarioFixture _usuarioFixture;
        public SenhaTests()
        {
            _faker = new Faker();
            _usuarioFixture = new UsuarioFixture();
        }
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

        [Fact(DisplayName = "Sucesso ao criar senha - senha válida")]
        [Trait("Categoria", "Usuario Tests")]
        public void CriaSenha_SenhaValida_DeveCriarComSucesso()
        {
            //Arrange
            var senha = "Teste@213";
            //Act
            var novaSenhaVO = new Senha(senha);
            //Assert
            Assert.Equal(novaSenhaVO.Hash, senha);

        }

        [Fact(DisplayName = "Falha ao criar senha do usuário - senha não preenchida")]
        [Trait("Categoria", "Senha Tests")]
        public void AtualizarSenhaUsuario_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new Senha(string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaObrigatoria, result.Message);
        }
        /// <summary>
        /// Testa falha quando a senha não atende aos requisitos de força.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de senha fraca.
        /// </summary>
        [Theory(DisplayName = "Falha ao cadastrar novo usuário - senha fraca")]
        [Trait("Categoria", "Senha Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void CadastrarUsuarioJogador_SenhaFraca_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => new Senha(senhaInvalida));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }
    }
}
