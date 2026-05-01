using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
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
            var nomeJogo = new NomeJogo("Jogo Teste");
            var descricaoJogo = new Descricao(_faker.Random.String(100));
            var precoNoMomento = new Preco(_faker.Random.Decimal(1, 100));
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
            Assert.Equal(MensagensDominio.ValorInvalido, result.Message);
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
            Assert.Equal(MensagensDominio.JogoAtivo, result.Message);
        }

        [Fact(DisplayName = "Atualizar jogo - jogo válido deve atualizar jogo com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_JogoValido_DeveAtualizarJogoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            var novoNome = new NomeJogo("Novo Nome");
            var novaDescricao = new Descricao("Nova Descrição");
            var novoPreco = new Preco(20.0m);
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
            var novoNome = new NomeJogo("Novo Nome");
            var novaDescricao = new Descricao("Nova Descrição");
            var novoPreco = new Preco(20.0m);
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
            var novoPreco = new Preco(20.0m);
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
            var novoPreco = new Preco(20.0m);
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
            var novoPreco = new Preco(20.0m);
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
            var novoPreco = new Preco(20.0m);
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
            //Act
            var result = Assert.Throws<DomainException>(() => new Preco(_faker.Random.Decimal(-1, 0)));
            //Assert
            Assert.Equal(MensagensDominio.ValorInvalido, result.Message);
        }

        [Fact(DisplayName = "Adiciona promoção - deve adicionar promoção com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicionaPromocaoJogo_JogoValido_DeveAdicionarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.Preco;
            //Act
            jogo.AdicionarPromocao(100, DateTime.UtcNow.AddMonths(2));
            //Assert
            Assert.Contains(jogo.Promocoes, p => p.Valor == 100);
        }

        [Fact(DisplayName = "Falha ao adicionar promoção - valor promoção maior ou igual que o valor base")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicionaPromocaoJogo_ValorInvalido_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.Preco;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.AdicionarPromocao(150, DateTime.UtcNow.AddMonths(2)));
            //Assert
            Assert.Contains(MensagensDominio.PromocaoValorMaior, result.Message);
        }


        [Fact(DisplayName = "Desativar promoção - deve desativar uma promoção com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void DesativarPromocaoJogo_JogoValido_DeveDesativarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.Preco;
            //Act
            jogo.AdicionarPromocao(100.00m, DateTime.UtcNow.AddDays(10));
            var promocao = jogo.Promocoes.FirstOrDefault(x => x.JogoId == jogo.Id);
            jogo.DesativarPromocao(promocao.Id);
            //Assert
            Assert.Contains(jogo.Promocoes, p => !p.Ativo);
        }

        [Fact(DisplayName = "Desativar promoção - falha ao tentar desativar em lista vazia")]
        [Trait("Categoria", "Jogos Tests")]
        public void DesativarPromocaoJogo_ListaVazia_DeveDesativarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var idInexistente = Guid.NewGuid();
            //Act            
            var result = Assert.Throws<DomainException>(() => jogo.DesativarPromocao(idInexistente));
            //Assert
            Assert.Equal(MensagensDominio.PromocaoNaoEncontrada, result.Message);
        }
    }
}
