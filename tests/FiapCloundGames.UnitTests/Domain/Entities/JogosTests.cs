using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Domain.Entities
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
            var jogo = new Jogo(nomeJogo, descricaoJogo, precoNoMomento, GeneroJogo.Acao);
            //Assert
            Assert.Equal(nomeJogo, jogo.Nome);
            Assert.Equal(descricaoJogo, jogo.Descricao);
            Assert.Equal(precoNoMomento, jogo.PrecoBase);
            Assert.True(jogo.Ativo);
            Assert.NotEqual(default, jogo.DataCadastro);
            Assert.NotEqual(Guid.Empty, jogo.Id);
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
            Assert.Equal(novoPreco, jogo.PrecoBase);
            Assert.Equal(novoGenero, jogo.Genero);
        }


        [Fact(DisplayName = "Falha ao atualizar jogo - jogo inativo deve lançar exceção")]
        [Trait("Categoria", "Jogos Tests")]
        public void AtualizarJogo_JogoInativo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosInativo();
            var novoNome = new NomeJogo( "Novo Nome");
            var novaDescricao = new Descricao( "Nova Descrição");
            var novoPreco = new Preco(20.0m);
            GeneroJogo novoGenero = GeneroJogo.Aventura;
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.Atualizar(novoNome, novaDescricao, novoPreco, novoGenero));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);
        }

        [Fact(DisplayName = "Adiciona promoção - deve adicionar promoção com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicionaPromocaoJogo_JogoValido_DeveAdicionarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.PrecoBase;
            var valorPromocaoVO = new Preco(100.00m);
            var periodoVO = new Periodo(DateTime.UtcNow.AddMonths(2));
            //Act
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            //Assert
            Assert.Contains(jogo.Promocoes, p => p.ValorPromocao.Valor == 100);
        }

        [Fact(DisplayName = "Falha ao adicionar promoção - valor promoção maior ou igual que o valor base")]
        [Trait("Categoria", "Jogos Tests")]
        public void AdicionaPromocaoJogo_ValorMaiorQueBase_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.PrecoBase;
            var valorPromocaoVO = new Preco(200.00m);
            var periodoVO = new Periodo(DateTime.UtcNow.AddMonths(2));
            //Act
            var result = Assert.Throws<DomainException>(() => jogo.AdicionarPromocao(valorPromocaoVO, periodoVO));
            //Assert
            Assert.Contains(MensagensDominio.PromocaoValorMaior, result.Message);
        }


        [Fact(DisplayName = "Sucesso ao alterar valor promocao - valor promoção alterado")]
        [Trait("Categoria", "Jogos Tests")]
        public void AlteraValorPromocaoJogo_PromocaoValida_DeveAlterarComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.PrecoBase;
            var valorPromocaoVO = new Preco(100.00m);
            var novoValorPromocaoVO = new Preco(75.00m);
            var novaData = DateTime.UtcNow.AddMonths(2);
            var periodoVO = new Periodo(DateTime.UtcNow.AddMonths(2));
            var novaPromocao = new Promocao(jogo.Id, novoValorPromocaoVO, periodoVO);
            //Act
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            var promocao = jogo.Promocoes.First();
            jogo.AlteraPromocao(promocao.Id, novoValorPromocaoVO, novaData);
            //Assert
            Assert.NotEqual(novoValorPromocaoVO, valorPromocaoVO);
        }


        [Fact(DisplayName = "Falha ao alterar valor promocao - novo valor promoção maior que o valor base")]
        [Trait("Categoria", "Jogos Tests")]
        public void AlteraValorPromocaoJogo_NovoValorMaiorQueBase_DeveLancarComExcecao()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.PrecoBase;
            var valorPromocaoVO = new Preco(100.00m);
            var novoValorPromocaoVO = new Preco(175.00m);
            var novaData = DateTime.UtcNow.AddMonths(2);
            var periodoVO = new Periodo(DateTime.UtcNow.AddMonths(2));
            //Act
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            var promocao = jogo.Promocoes.First();
            var result = Assert.Throws<DomainException>(() => jogo.AlteraPromocao(promocao.Id, novoValorPromocaoVO, novaData));
            //Assert
            Assert.Equal(MensagensDominio.PromocaoValorMaior, result.Message);
        }


        [Fact(DisplayName = "Desativar promoção - deve desativar uma promoção com sucesso")]
        [Trait("Categoria", "Jogos Tests")]
        public void DesativarPromocaoJogo_JogoValido_DeveDesativarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogoFixture.ObtemJogosParaPromocao();
            var precoBase = jogo.PrecoBase;
            var valorPromocaoVO = new Preco(100.00m);
            var periodoVO = new Periodo(DateTime.UtcNow.AddDays(10));
            //Act
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
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
