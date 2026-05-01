using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class NomeJogoTests
    {
        private Faker _faker;
        public NomeJogoTests()
        {
            _faker = new Faker();   
        }

        [Fact(DisplayName = "Sucesso ao criar nome jogo - deve criar com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void CriaNome_NomeValido_DeveCriarNomeComSucesso()
        {
            //Arrange
            var nomeJogo = "Red Dead Redptiom 2";
            //Act 
            var nomeVO = new NomeJogo(nomeJogo);
            //Assert
            Assert.Equal(nomeVO.Valor,nomeJogo);
        }

        [Fact(DisplayName = "Falha ao criar nome do jogo - nome não preenchido")]
        [Trait("Categoria", "Jogos Tests")]
        public void CriaNome_NomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new NomeJogo(string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.JogoNomeObrigatorio, result.Message);
        }

        [Fact(DisplayName = "Falha ao criar nome do jogo - nome inválido")]
        [Trait("Categoria", "Jogos Tests")]
        public void CriaNome_NomeInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new NomeJogo(_faker.Random.String(41)));
            //Assert
            Assert.Equal(MensagensDominio.JogoTamanhoNomeInvalido, result.Message);
        }
    }
}
