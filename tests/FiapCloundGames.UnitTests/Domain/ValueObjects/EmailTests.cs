using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class EmailTests
    {
        private readonly Faker _faker;
        public EmailTests()
        {
            _faker = new Faker();   
        }
        [Fact(DisplayName = "Sucesso ao criar email - deve criar email com sucesso")]
        [Trait("Categoria", "Email Tests")]
        public void EmailCria_EmailValido_DeveCriarComSucesso()
        {
            //Arrange
            var email = _faker.Internet.Email();
            //Act
            var emailVO = new Email(email);
            //Assert
            Assert.Equal(email, emailVO.Valor);
        }

        [Fact(DisplayName = "Falha ao criar email - email não preenchido")]
        [Trait("Categoria", "Email Tests")]
        public void EmailCria_EmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new Email(string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.EmailObrigatorio, result.Message);
        }

        [Theory(DisplayName = "Falha ao criar email - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void EmailCria_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new Email(emailInvalido));
            //Assert    
            Assert.Equal(MensagensDominio.EmailInvalido, result.Message);
        }
    }
}
