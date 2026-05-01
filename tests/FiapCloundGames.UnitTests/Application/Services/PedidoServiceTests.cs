using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

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
        [Fact(DisplayName ="Sucesso ao criar pedido - pedido criado com sucesso")]
        [Trait("Categoria","Pedido Service Tests")]
        public async Task RealizaPedido_PedidoValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogoPedido = _jogosFixture.ObtemJogosComSucesso();
            List<Guid> lista = new List<Guid>();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var pedidoMock = new Mock<IPedidoRepository>();
            usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            var pedidoService = new PedidoService(usuarioMock.Object, pedidoMock.Object);
            //Act
            lista.Add(jogoPedido.Id);
            await pedidoService.RealizarPedido(usuario.Id, lista);
            //Assert
            pedidoMock.Verify(p => p.Adicionar(It.IsAny<Pedido>), Times.Once);
        }
    }
}
