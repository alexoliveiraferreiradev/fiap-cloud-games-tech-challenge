using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class DescricaoTests
    {
        private readonly Faker _faker;
        public DescricaoTests()
        {
            _faker = new Faker();   
        }
        [Fact(DisplayName = "Sucesso ao criar descricao jogo - descrição no limite")]
        [Trait("Categoria", "Descricao Tests")]
        public void CriaDescricao_DescricaoNoLimite_DevePassar()
        {
            //Act
            var nomeJogoVO = new NomeJogo("Teste Jogo");
            var descricaoNoLimiteVO = new Descricao( _faker.Random.String(100));
            var precoVO = new Preco(10.0m);
            //Arrange
            var jogo = new Jogos(nomeJogoVO, descricaoNoLimiteVO, precoVO, GeneroJogo.Acao);
            //Assert
            Assert.Equal(100, jogo.Descricao.Valor.Length);
        }

        [Fact(DisplayName = "Falha ao criar descrição jogo - descrição do jogo inválida deve lançar exceção")]
        [Trait("Categoria", "Descricao Tests")]
        public void CriaDescricaoJogo_DescricaoJogoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var novaDescricao = "N";
            //Act
            var result = Assert.Throws<DomainException>(() => new Descricao(novaDescricao));
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoTamanhoInvalido, result.Message);
        }
    }
}
