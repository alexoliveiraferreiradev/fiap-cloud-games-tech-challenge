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
            var jogo = new Jogos(nomeJogo, descricaoJogo, precoNoMomento, GeneroJogo.Acao);
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

        [Fact(DisplayName = "Desativar jogo - jogo ativo deve desativar jogo com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void DesativarJogo_JogoAtivo_DeveDesativarJogoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            //Act
            jogo.Desativar();
            //Assert
            Assert.False(jogo.Ativo);
        }

        [Fact(DisplayName = "Desativar jogo - jogo inativo deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void DesativarJogo_JogoInativo_DeveDesativarJogoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Desativar());
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }


        [Fact(DisplayName = "Reativar jogo - deve reativar jogo com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void ReativarJogo_JogoInativo_DeveReativarJogoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            //Act
            jogo.Reativar();
            //Assert
            Assert.True(jogo.Ativo);
        }


        [Fact(DisplayName = "Falha ao reativar jogo - jogo já ativo deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void ReativarJogo_JogoAtivo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Reativar());
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Atualizar jogo - jogo válido deve atualizar jogo com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_JogoValido_DeveAtualizarJogoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            var novoNome = "Novo Nome";
            var novaDescricao = "Nova Descrição";
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero);
            //Assert
            Assert.Equal(novoNome, jogo.Nome);
            Assert.Equal(novaDescricao, jogo.Descricao);
            Assert.Equal(novoPreco, jogo.Preco);
            Assert.Equal(novoGenero, jogo.Genero);
        }


        [Fact(DisplayName = "Falha ao atualizar jogo - jogo inativo deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_JogoInativo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = "Novo Nome";
            var novaDescricao = "Nova Descrição";
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }


        [Fact(DisplayName = "Falha ao atualizar jogo - novo nome inválido deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_NovoNomeJogoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = "N";
            var novaDescricao = "Nova Descrição";
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar jogo - novo nome não preenchido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_NovoNomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = string.Empty;
            var novaDescricao = "Nova Descrição";
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar jogo - nova descrição do jogo inválida deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_NovaDescricaoJogoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = "Read Dead 2";
            var novaDescricao = "N";
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }
        [Fact(DisplayName = "Falha ao atualizar jogo - nova descrição do jogo não preenchida")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_NovaDescricaoNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = "Read Dead 2";
            var novaDescricao = string.Empty;
            var novoPreco = 20.0m;
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar jogo - novo preço do jogo inválido")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_NovaPrecoJogoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = "Read Dead 2";
            var novaDescricao = _faker.Random.String(20);
            var novoPreco = _faker.Random.Decimal(-1, 0);
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Adiciona promoção - deve adicionar promoção com sucesso")]
        [Trait("Categoria","Jogos Tests")]
        public void AdicionaPromocaoJogo_JogoValido_DeveAdicionarPromocaoComSucesso()
        {
            
        }
    }
}
