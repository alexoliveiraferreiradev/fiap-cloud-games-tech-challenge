using Bogus;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class JogosServiceTests
    {
        private readonly Faker _faker;
        private readonly JogosFixture _jogosFixture;
        public JogosServiceTests()
        {
            _faker = new Faker();
            _jogosFixture = new JogosFixture();
        }

        [Fact(DisplayName = "Adicionar jogo - deve criar um jogo com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_JogoValido_DeveAdicionarJogoComSucesso()
        {
            //Arrange
            var request = new CriarJogoRequest("Halo", "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await service.CriaJogo(request);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.Nome, result.Nome);

            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - nome jogo não preenchido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_NomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest(string.Empty, "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoNomeObrigatorio, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - nome jogo inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_NomeInvalido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest(_faker.Random.String(21), "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoTamanhoNomeInvalido, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - descrição não preenchida")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_DescricaoNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", string.Empty, 150.00m, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoObrigatoria, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - descrição inválida")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_DescricaoInvalida_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", _faker.Random.String(101), 150.00m, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoTamanhoInvalido, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - gênero inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_GeneroInvalido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", "Jogo de tiro", 150.00m, (GeneroJogo)999);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoGeneroObrigatorio, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }



        [Fact(DisplayName = "Atualizar jogo - preço inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_PrecoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", "Jogo de tiro", -1, GeneroJogo.FPS);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoPrecoInvalido, result.Message);
            repoMock.Verify(r => r.Adicionar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Atualizar jogo - deve atualizar o jogo com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AtualizarJogo_JogoValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosComSucesso();
            var request = new UpdateJogosRequest("Read Dead 2", "Jogo de tiro", 10.00m, GeneroJogo.Aventura);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await service.AtualizarJogo(jogo.Id, request);
            //Assert
            Assert.Equal(jogo.Nome, request.novoNome);
            Assert.Equal(jogo.Descricao, request.novaDescricao);
            Assert.Equal(jogo.Preco, request.novoPreco);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Jogos>()), Times.Once);
        }

        [Fact(DisplayName = "Falha ao atualizar jogo - jogo não encontrado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AtualizarJogo_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrange
            var jogoId = Guid.NewGuid();
            var request = new UpdateJogosRequest("Read Dead 2", "Jogo de tiro", 10.00m, GeneroJogo.Aventura);
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(jogoId)).ReturnsAsync((Jogos)null);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.AtualizarJogo(jogoId, request));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);

            repoMock.Verify(r => r.Atualizar(It.IsAny<Jogos>()), Times.Never);
        }


        [Fact(DisplayName = "Desativar jogo - deve desativar com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarJogo_JogoValido_DeveDesativarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosComSucesso();
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await service.Desativar(jogo.Id);
            //Assert
            Assert.False(jogo.Ativo);

            repoMock.Verify(r => r.Atualizar(It.IsAny<Jogos>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao desativar jogo - jogo inativo")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarJogo_JogoInativo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosInativo();
            //Mock
            var repoMock = new Mock<IJogosRepository>();
            var service = new JogosService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.Desativar(jogo.Id));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);

            repoMock.Verify(r => r.Atualizar(It.IsAny<Jogos>()), Times.Never);
        }


    }
}
