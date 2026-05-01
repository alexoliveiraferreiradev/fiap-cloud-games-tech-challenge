using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class NomeTests
    {
        private readonly Faker _faker;
        public NomeTests()
        {
            _faker = new Faker();
        }
        [Fact(DisplayName = "Sucesso ao criar nome - deve criar nome com sucesso")]
        [Trait("Categoria", "Nome Tests")]
        public void NomeCria_NomeValido_DeveCriarComSucesso()
        {
            //Arrange
            var nome = _faker.Internet.UserName();
            //Act
            var nomeVO = new Nome(nome);
            //Assert
            Assert.Equal(nome, nomeVO.Valor);
        }
        [Fact(DisplayName = "Falha ao criar novo nome - nome não preenchido")]
        [Trait("Categoria", "Nome Tests")]
        public void NomeCria_NomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => new Nome(string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNomeObrigatorio, result.Message);
        }

        /// <summary>
        /// Testa falha na criação quando nome de usuário é inválido
        /// </summary>
        /// <param name="nomeInvalido"></param>
        [Theory(DisplayName = "Falha ao cadastrar novo Usuário - nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("aB")]
        [InlineData("xD")]
        [InlineData("aBuhdoiumnaitionahjdrt")]
        public void NomeCria_NomeInvalido_DeveLancarExcecao(string nomeInvalido)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => new Nome(nomeInvalido));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioTamanhoNomeInvalido, result.Message);
        }

    }
}
