using Bogus;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class JogosServiceTests
    {
        private readonly Faker _faker;
        private readonly Mock<IJogosRepository> _mockJogo;
        private readonly JogosService _jogosService;
        private readonly JogosFixture _jogosFixture;
        private readonly PromocaoFixture _promocaoFixture;
        public JogosServiceTests()
        {
            _faker = new Faker();
            _mockJogo = new Mock<IJogosRepository>();
            _jogosService = new JogosService(_mockJogo.Object);
            _jogosFixture = new JogosFixture();
            _promocaoFixture = new PromocaoFixture();
        }

        [Fact(DisplayName = "Adicionar jogo - deve criar um jogo com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_JogoValido_DeveAdicionarJogoComSucesso()
        {
            //Arrange
            var request = new CriarJogoRequest("Halo", "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Mock
            //Act 
            var result = await _jogosService.CriaJogo(request);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(request.Nome, result.Nome.Valor);

            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - nome jogo não preenchido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_NomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest(string.Empty, "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - nome jogo inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_NomeInvalido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest(_faker.Random.String(41), "Jogo de tiro", 150.00m, GeneroJogo.FPS);
            //Mock
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - nome duplicado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_NomeDuplicado_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", "Melhor jogo do mundo", 150.00m, GeneroJogo.Aventura);
            var jogoExistente = _jogosFixture.ObtemJogosComSucesso();

            _mockJogo.Setup(r => r.ObtemPorNome(request.Nome)).ReturnsAsync(jogoExistente);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoMesmoNomeExistente, result.Message);
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo - descrição não preenchida")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_DescricaoNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", string.Empty, 150.00m, GeneroJogo.FPS);

            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoObrigatoria, result.Message);
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - descrição inválida")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_DescricaoInvalida_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", _faker.Random.String(101), 150.00m, GeneroJogo.FPS);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoDescricaoTamanhoInvalido, result.Message);
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo - gênero inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarJogo_GeneroInvalido_DeveLancarExcecao()
        {
            //Arrange
            var request = new CriarJogoRequest("Read Dead 2", "Jogo de tiro", 150.00m, (GeneroJogo)999);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.CriaJogo(request));
            //Assert
            Assert.Equal(MensagensDominio.JogoGeneroObrigatorio, result.Message);
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }



        [Fact(DisplayName = "Atualizar jogo - preço inválido")]
        [Trait("Categoria", "JogosService Tests")]
        public void AdicionarJogo_PrecoInvalido_DeveLancarExcecao()
        {
            //Arrange
            //Act 
            var result = Assert.Throws<DomainException>(() => new Preco(-1));
            //Assert
            Assert.Equal(MensagensDominio.ValorInvalido, result.Message);
            _mockJogo.Verify(r => r.Adicionar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Atualizar jogo - deve atualizar o jogo com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AtualizarJogo_JogoValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosComSucesso();
            var request = new UpdateJogosRequest("Read Dead 2", "Jogo de tiro", 10.00m, GeneroJogo.Aventura);

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.AtualizarJogo(jogo.Id, request);
            //Assert
            Assert.Equal(jogo.Nome.Valor, request.novoNome);
            Assert.Equal(jogo.Descricao.Valor, request.novaDescricao);
            Assert.Equal(jogo.PrecoBase.Valor, request.novoPreco);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Once);
        }

        [Fact(DisplayName = "Falha ao atualizar jogo - jogo não encontrado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AtualizarJogo_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrange
            var jogoId = Guid.NewGuid();
            var request = new UpdateJogosRequest("Read Dead 2", "Jogo de tiro", 10.00m, GeneroJogo.Aventura);

            _mockJogo.Setup(r => r.ObterPorId(jogoId)).ReturnsAsync((Jogo)null);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.AtualizarJogo(jogoId, request));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);

            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Desativar jogo - deve desativar com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarJogo_JogoValido_DeveDesativarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosComSucesso();

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.Desativar(jogo.Id);
            //Assert
            Assert.False(jogo.Ativo);

            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao desativar jogo - jogo inativo")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarJogo_JogoInativo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosInativo();

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.Desativar(jogo.Id));
            //Assert
            Assert.Equal(MensagensDominio.JogoInvalido, result.Message);

            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao desativar jogo - jogo não encontrado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarJogo_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrange
            var jogoId = Guid.NewGuid();

            _mockJogo.Setup(r => r.ObterPorId(jogoId)).ReturnsAsync((Jogo)null);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.Desativar(jogoId));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Never);
        }

        [Fact(DisplayName = "Reativar jogo - deve reativar jogo com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task ReativarJogo_JogoValido_DeveReativarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosInativo();

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.Reativar(jogo.Id);
            //Assert
            Assert.True(jogo.Ativo);

            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao reativar jogo - jogo ativo")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task ReativarJogo_JogoAtivo_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosComSucesso();

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.Reativar(jogo.Id));
            //Assert
            Assert.Equal(MensagensDominio.JogoAtivo, result.Message);

            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Adicionar promocão - deve adicionar promoção com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarPromocao_JogoValido_DeveAdicionarPromocaoComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var criaPromocaoRequest = new CriaPromocaoRequest(jogo.Id, 90.00m, DateTime.UtcNow.AddDays(10));

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.AdicionarPromocao(criaPromocaoRequest);
            //Assert
            Assert.Contains(jogo.Promocoes, p => p.Ativo);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Once);
        }

        [Fact(DisplayName = "Falha ao adicionar promocão - jogo não encontrado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AdicionarPromocao_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var criaPromocaoRequest = new CriaPromocaoRequest(jogo.Id, 90.00m, DateTime.UtcNow.AddDays(10));

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync((Jogo)null);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.AdicionarPromocao(criaPromocaoRequest));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.Never);
        }


        [Fact(DisplayName = "Sucesso ao alterar valor promocão - valor promoção alterado")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AlteraValorPromocao_PromocaoValida_DeveAlterarComSucesso()
        {
            //Arrange         
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var dataFim = DateTime.UtcNow.AddDays(11);

            var criaPromocaoRequest = new CriaPromocaoRequest(jogo.Id, 90.00m, DateTime.UtcNow.AddDays(10));
            var updatePromocaoRequest = new UpdatePromocaoRequest(jogo.Id,70.00m, dataFim);

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            await _jogosService.AdicionarPromocao(criaPromocaoRequest);

            var promocaoExistente = jogo.Promocoes.First();
            //Mock           
            _mockJogo.Setup(r => r.ObterPromocaoPorId(promocaoExistente.Id)).ReturnsAsync(promocaoExistente);
            //Act 
            await _jogosService.AtualizaPromocao(promocaoExistente.Id,updatePromocaoRequest);            
            //Assert
            Assert.NotEqual(updatePromocaoRequest.novoValorPromocao, criaPromocaoRequest.valorPromocao);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Falha ao alterar promocão - promoção não encotrada")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task AlteraValorPromocao_PromocaoNaoEncontrada_DeveLancarExcecao()
        {
            //Arrange         
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var dataFim = DateTime.UtcNow.AddDays(11);

            var criaPromocaoRequest = new CriaPromocaoRequest(jogo.Id, 90.00m, DateTime.UtcNow.AddDays(10));
            var updatePromocaoRequest = new UpdatePromocaoRequest(jogo.Id,70.00m, dataFim);

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            await _jogosService.AdicionarPromocao(criaPromocaoRequest);

            var promocaoExistente = jogo.Promocoes.First();
            //Mock           
            _mockJogo.Setup(r => r.ObterPromocaoPorId(promocaoExistente.Id)).ReturnsAsync(promocaoExistente);
            //Act 
            await _jogosService.AtualizaPromocao(promocaoExistente.Id,updatePromocaoRequest);            
            //Assert
            Assert.NotEqual(updatePromocaoRequest.novoValorPromocao, criaPromocaoRequest.valorPromocao);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Desativar promocão - deve desativar com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarPromocao_JogoValido_DeveDesativarComSucesso()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var promocaoRequest = _promocaoFixture.ObtemPromacaoRequest(jogo.Id);

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.AdicionarPromocao(promocaoRequest);
            var idPromocao = jogo.Promocoes.Where(x => x.JogoId == jogo.Id).Select(x => x.Id).First();
            await _jogosService.DesativarPromocao(jogo.Id, idPromocao);

            //Assert
            var promocaoDesativada = jogo.Promocoes.First(p => p.Id == idPromocao);
            Assert.False(promocaoDesativada.Ativo);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.AtLeastOnce);
        }

        [Fact(DisplayName = "Falha ao desativar promocão - promoção não encontrada")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task DesativarPromocao_PromocaoNaoEncontrada_DeveLancarExcecao()
        {
            //Arrange
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var promocaoRequest = _promocaoFixture.ObtemPromacaoRequest(jogo.Id);
            var idInexistente = Guid.NewGuid();

            _mockJogo.Setup(r => r.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act 
            await _jogosService.AdicionarPromocao(promocaoRequest);
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _jogosService.DesativarPromocao(jogo.Id, idInexistente));
            //Assert
            Assert.Equal(MensagensDominio.PromocaoNaoEncontrada, result.Message);
            _mockJogo.Verify(r => r.Atualizar(It.IsAny<Jogo>()), Times.AtMostOnce);
        }

        [Fact(DisplayName = "Obtém catálogo de jogos com sucesso")]
        [Trait("Categoria", "JogosService Tests")]
        public async Task ObtemCatalagoJogos_DeveObterComSucesso()
        {
            //Arrange
            var listaJogosAtivos = new List<Jogo>();
            var jogoAtivo = _jogosFixture.ObtemJogosParaPromocao();
            listaJogosAtivos.Add(jogoAtivo);

            _mockJogo.Setup(r => r.ObtemJogosAtivos()).ReturnsAsync(listaJogosAtivos);
            //Act 
            var respone = await _jogosService.ObtemCatalagoJogos();
            //Assert
            Assert.Contains(respone, r => r.Id == jogoAtivo.Id);
        }

    }
}
