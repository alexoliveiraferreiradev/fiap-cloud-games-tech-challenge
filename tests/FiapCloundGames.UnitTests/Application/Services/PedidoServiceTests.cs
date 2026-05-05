using AutoMapper;
using Castle.Core.Logging;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Configuration.Mapping;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.Net.NetworkInformation;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class PedidoServiceTests
    {
        private JogosFixture _jogosFixture;
        private UsuarioFixture _usuarioFixture;
        private readonly IMapper _mapper;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;
        private readonly Mock<IJogoRepository> _jogoMock;
        private readonly Mock<IBibliotecaService> _bibliotecaMock;
        private readonly Mock<IPedidoRepository> _pedidoMock;
        private readonly PedidoService _service;
        private readonly ILogger<PedidoService> _logger;

        public PedidoServiceTests()
        {
            _jogosFixture = new JogosFixture();
            _usuarioFixture = new UsuarioFixture();

            var configMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<PedidoProfile>();
            });
            _mapper = configMapper.CreateMapper();
            _bibliotecaMock = new Mock<IBibliotecaService>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _jogoMock = new Mock<IJogoRepository>();
            _pedidoMock = new Mock<IPedidoRepository>();
            _logger = NullLogger<PedidoService>.Instance;

            _service = new PedidoService(_pedidoMock.Object,_jogoMock.Object,
                _usuarioRepositoryMock.Object,_bibliotecaMock.Object,_mapper,_logger);
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

            lista.Add(jogoPedido.Id);
            _usuarioRepositoryMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            _jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
            .ReturnsAsync(new List<Jogo> { jogoPedido });
            //Act
            var result = await _service.RealizarPedido(usuario.Id, lista);
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, result.Status);
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);
        }


        [Fact(DisplayName = "Sucesso ao realizar pedido - mais de um jogo")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_PedidoValidoMaisDeUmJogo_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            //Mock

            _usuarioRepositoryMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
                _jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Jogo> { jogoPedido });
                lista.Add(jogoPedido.Id);
            }
           
            //Act
            var result = await _service.RealizarPedido(usuario.Id, lista);
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, result.Status);
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);
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
            _jogoMock.Setup(r => r.ObterPorId(jogoPedido.Id)).ReturnsAsync(jogoPedido);
            lista.Add(jogoPedido.Id);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async ()=> await _service.RealizarPedido(usuarioId, lista));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao realizar pedido - jogo inativo")]
        [Trait("Categoria", "Pedido Service Tests")]
        public async Task RealizaPedido_JogoInativo_DeveLancarComExcecao()
        {
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            List<Guid> lista = new List<Guid>();
            var jogoPedido = _jogosFixture.ObtemJogosInativo();
            //Mock

            lista.Add(jogoPedido.Id);
            _usuarioRepositoryMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            _jogoMock.Setup(r => r.ObterPorId(jogoPedido.Id)).ReturnsAsync(jogoPedido);            
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.RealizarPedido(usuario.Id, new List<Guid> { jogoPedido.Id }));
            //Assert
            Assert.Contains("Não foi possível realizar o pedido", result.Message);
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
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
            _usuarioRepositoryMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            lista.Add(jogoId);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async ()=> await _service.RealizarPedido(usuario.Id, lista));
            //Assert
            Assert.Contains("Não foi possível realizar o pedido", result.Message);
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Never);
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
            _usuarioRepositoryMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);

            _jogoMock.Setup(r => r.ObterJogosPorIds(It.IsAny<List<Guid>>()))
                    .ReturnsAsync(new List<Jogo> { jogo });
            
            
            _bibliotecaMock.Setup(s => s.ObterIdsJogosDoUsuario(usuario.Id))
                                 .ReturnsAsync(new List<Guid>());

            
            var result = await _service.RealizarPedido(usuario.Id, listaIds);

            // Assert
            Assert.Equal("Finalizado", result.Status.ToString()); 
            Assert.Equal(50.0m, result.ValorTotal);

            // Verifica se o pedido foi realmente adicionado ao repositório
            _pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);

            // Verifica se liberou os jogos na biblioteca
            _bibliotecaMock.Verify(s => s.LiberarJogosAposPedido(usuario.Id, It.IsAny<List<Guid>>()), Times.Once);
        }
    }
}
