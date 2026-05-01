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

        [Fact(DisplayName = "Falha ao criar senha - senha não preenchida")]
        [Trait("Categoria", "Senha Tests")]
        public void CriaSenha_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new Senha(string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }
        /// <summary>
        /// Testa falha quando a senha não atende aos requisitos de força.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de senha fraca.
        /// </summary>
        [Theory(DisplayName = "Falha ao criar senha - senha fraca")]
        [Trait("Categoria", "Senha Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void CriaSenha_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => new Senha(senhaInvalida));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }
    }
}
