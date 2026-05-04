using AutoMapper;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;
using System.Net.NetworkInformation;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class PedidoServiceTests
    {
        private JogosFixture _jogosFixture;
        private UsuarioFixture _usuarioFixture;
        private readonly IMapper _mapper;
        public PedidoServiceTests()
        {
            _jogosFixture = new JogosFixture();
            _usuarioFixture = new UsuarioFixture();
        }
        [Fact(DisplayName = "Sucesso ao realizar pedido - pedido criado com sucesso")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_PedidoValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
            //Mock

            var usuarioMock = new Mock<IUsuarioRepository>();
            var jogoMock = new Mock<IJogoRepository>();
            var bibliotecaService = new Mock<IBibliotecaService>();
            lista.Add(jogoPedido.Id);
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Jogo> { jogoPedido });
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object, usuarioMock.Object, bibliotecaService.Object);
            //Act
            var result = await pedidoService.RealizarPedido(usuario.Id, lista);
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, result.Status);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);
        }


        [Fact(DisplayName = "Sucesso ao realizar pedido - mais de um jogo")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_PedidoValidoMaisDeUmJogo_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaService = new Mock<IBibliotecaService>();
            var jogoMock = new Mock<IJogoRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
                jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Jogo> { jogoPedido });
                lista.Add(jogoPedido.Id);
            }
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object, usuarioMock.Object, bibliotecaService.Object);
            //Act
            var result = await pedidoService.RealizarPedido(usuario.Id, lista);
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, result.Status);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao realizar pedido - usuário não encontrado")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_UsuarioNaoEncontrado_DeveLancarComExcecao()
        {
            //Arrange
            var usuarioId = Guid.NewGuid();
            List<Guid> lista = new List<Guid>();
            var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaService = new Mock<IBibliotecaService>();
            var jogoMock = new Mock<IJogoRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();
            jogoMock.Setup(r => r.ObterPorId(jogoPedido.Id)).ReturnsAsync(jogoPedido);
            lista.Add(jogoPedido.Id);
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object, usuarioMock.Object,bibliotecaService.Object);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async ()=> await pedidoService.RealizarPedido(usuarioId, lista));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao realizar pedido - jogo inativo")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_JogoInativo_DeveLancarComExcecao()
        {
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            var jogoPedido = _jogosFixture.ObtemJogosInativo();
            //Mock

            var usuarioMock = new Mock<IUsuarioRepository>();
            var jogoMock = new Mock<IJogoRepository>();
            var bibliotecaService = new Mock<IBibliotecaService>();
            lista.Add(jogoPedido.Id);
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            jogoMock.Setup(r => r.ObterPorId(jogoPedido.Id)).ReturnsAsync(jogoPedido);
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object, usuarioMock.Object,bibliotecaService.Object);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => pedidoService.RealizarPedido(usuario.Id, new List<Guid> { jogoPedido.Id }));
            //Assert
            Assert.Contains("Não foi possível realizar o pedido", result.Message);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao realizar pedido - jogo não encontrado")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_JogoNaoEncontrado_DeveLancarComExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            var jogoId = Guid.NewGuid();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaService = new Mock<IBibliotecaService>();
            var jogoMock = new Mock<IJogoRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            lista.Add(jogoId);
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object, usuarioMock.Object,bibliotecaService.Object);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async ()=> await pedidoService.RealizarPedido(usuario.Id, lista));
            //Assert
            Assert.Contains("Não foi possível realizar o pedido", result.Message);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
        }
        [Fact(DisplayName = "Sucesso ao realizar pedido - jogo com promoção")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_JogoComPromocao_DeveRealizarComSucesso()
        {
            // Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogo = _jogosFixture.ObtemJogosParaPromocao();
            var listaIds = new List<Guid> { jogo.Id };

          
            var valorPromocao = new Preco(50.0m);
            var periodoPromocao = new Periodo(DateTime.UtcNow.AddDays(10));
            jogo.AdicionarPromocao(valorPromocao, periodoPromocao);

            // Mocks
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaServiceMock = new Mock<IBibliotecaService>();
            var jogoMock = new Mock<IJogoRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();

            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

            jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Jogo> { jogo });
            var pedidoService = new PedidoService(pedidoMock.Object,jogoMock.Object,usuarioMock.Object,bibliotecaServiceMock.Object);
            
            bibliotecaServiceMock.Setup(s => s.ObterIdsJogosDoUsuario(usuario.Id))
                                 .ReturnsAsync(new List<Guid>());

            
            var result = await pedidoService.RealizarPedido(usuario.Id, listaIds);

            // Assert
            Assert.Equal("Finalizado", result.Status.ToString()); 
            Assert.Equal(50.0m, result.ValorTotal.Valor);

            // Verifica se o pedido foi realmente adicionado ao repositório
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);

            // Verifica se liberou os jogos na biblioteca
            bibliotecaServiceMock.Verify(s => s.LiberarJogosAposPedido(usuario.Id, It.IsAny<List<Guid>>()), Times.Once);
        }
    }
}
