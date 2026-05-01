using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class PrecoTests
    {
        private Faker _faker;
        public PrecoTests()
        {
            _faker = new Faker();   
        }
        [Fact(DisplayName = "Criar preço - preço válido")]
        [Trait("Categoria", "Pedidos Tests")]
        public void CriaPromocao_PrecoValido_DevePassar()
        {
            //Arrange
            var precoValido = _faker.Random.Decimal(1,50);
            //Act 
            var precoVO = new Preco(precoValido);
            //Assert
            Assert.Equal(precoValido, precoVO.Valor);
        }

        [Fact(DisplayName = "Falha ao criar preço - preço inválido")]
        [Trait("Categoria", "Pedidos Tests")]
        public void CriaPromocao_PrecoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var precoInvalido = _faker.Random.Decimal(-1);
            //Act 
            var result = Assert.Throws<DomainException>(() => new Preco(precoInvalido));
            //Assert
            Assert.Equal(MensagensDominio.ValorInvalido, result.Message);
        }
    }
}
