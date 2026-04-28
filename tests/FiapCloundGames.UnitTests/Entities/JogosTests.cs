using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Entities
{
    public class JogosTests
    {
        private readonly Faker _faker;
        private readonly JogosFixture _jogoFixture;
        public JogosTests()
        {
            _faker = new Faker();
            _jogoFixture = new JogosFixture();
        }

        /// <summary>
        /// Valida a criação de um jogo com dados válidos, garantindo que as propriedades sejam atribuídas corretamente e que o jogo seja criado com sucesso.
        /// </summary>
        /// <remarks> Este teste verifica se um jogo é criado corretamente com nome, descrição e preço válidos. </remarks>
        [Fact(DisplayName = "Adicionar jogo - Deve criar um jogo com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_JogoValido_DeveAdicionarJogoComSucesso()
        {
            //Arrange
            var nomeJogo = "Jogo Teste";
            var descricaoJogo = _faker.Random.String(100);
            decimal precoNoMomento = _faker.Random.Decimal(1, 100);            
            //Act 
            var jogo = new Jogos(nomeJogo, descricaoJogo, precoNoMomento,GeneroJogo.Acao);
            //Assert
            Assert.Equal(nomeJogo, jogo.Nome);
            Assert.Equal(descricaoJogo, jogo.Descricao);
            Assert.Equal(precoNoMomento, jogo.Preco);
            Assert.True(jogo.Ativo);
            Assert.NotEqual(default, jogo.DataCadastro);
            Assert.NotEqual(Guid.Empty, jogo.Id);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - nome não preenchido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_NomeJogoNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosNomeNaoPreenchido());
            //Assert
            Assert.Equal(MensagensDominio.JogoNomeObrigatorio, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - descrição do jogo não preenchido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_DescricaoJogoNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosDescricaoNaoPreenchido());
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoObrigatoria, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - nome inválido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_NomeInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosNomeInvalido());
            //Assert
            Assert.Equal(MensagensDominio.JogoTamanhoNomeInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - descrição inválida")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_DescricaoInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosDescricaoInvalida());
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoTamanhoInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - preço inválido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_PrecoInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosPrecoInvalido());
            //Assert
            Assert.Equal(MensagensDominio.JogoPrecoInvalido, result.Message);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - gênero inválido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_GeneroInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => _jogoFixture.ObtemJogosGeneroInvalido());
            //Assert
            Assert.Equal(MensagensDominio.JogoGeneroObrigatorio, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - descrição no limite máximo")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicaoJogos_DescricaoNoLimite_DevePassar()
        {
            //Act
            var descricaoNoLimite = _faker.Random.String(100);
            //Arrange
            var jogo = new Jogos("Nome", descricaoNoLimite, 10.0m, GeneroJogo.Acao);
            //Assert
            Assert.Equal(100, jogo.Descricao.Length);
        }
    }
}
