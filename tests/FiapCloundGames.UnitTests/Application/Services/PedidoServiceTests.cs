using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;
using System.Net.NetworkInformation;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class PedidoServiceTests
    {
        private JogosFixture _jogosFixture;
        private UsuarioFixture _usuarioFixture;
        public PedidoServiceTests()
        {
            _jogosFixture = new JogosFixture();
            _usuarioFixture = new UsuarioFixture();
        }
        [Fact(DisplayName ="Sucesso ao realizar pedido - pedido criado com sucesso")]
        [Trait("Categoria","Pedido Service Tests")]
        public async Task RealizaPedido_PedidoValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
            List<Guid> lista = new List<Guid>();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var jogoMock = new Mock<IJogosRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            jogoMock.Setup(r => r.ObterPorId(jogoPedido.Id)).ReturnsAsync(jogoPedido);
            var pedidoService = new PedidoService(pedidoMock.Object, jogoMock.Object);
            //Act
            lista.Add(jogoPedido.Id);
            var result = await pedidoService.RealizarPedido(usuario.Id, lista);
            //Assert
            Assert.Equal(PedidoStatus.Finalizado, result.Status);
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>()), Times.Once);
        }
    }
}
