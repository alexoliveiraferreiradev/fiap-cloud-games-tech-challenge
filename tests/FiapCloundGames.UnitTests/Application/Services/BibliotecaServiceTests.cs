using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class BibliotecaServiceTests
    {
        private readonly JogosFixture _jogoFixture;
        private readonly UsuarioFixture _usuarioFixture;
        public BibliotecaServiceTests()
        {
            _jogoFixture = new JogosFixture();
            _usuarioFixture = new UsuarioFixture();
        }
        [Fact(DisplayName ="Sucesso ao adicionar jogo na biblioteca - adiciona jogo com sucesso")]
        [Trait("Categoria","Biblioteca Services")]
        public async Task AdicionaJogo_JogoValido_DeveAdicionarComSucesso()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            var criaJogoBibliotecaRequest = new CriaBibliotecaRequest(jogo.Nome.Valor, jogo.Descricao.Valor, jogo.Genero);
            //Mock
            var repoMock = new Mock<IBibliotecaRepository>();
            var usuarioMock = new Mock<IUsuarioRepository>();
            var jogoMock = new Mock<IJogosRepository>();
            var service = new BibliotecaService(repoMock.Object,usuarioMock.Object,jogoMock.Object);
            usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
            //Act
            await service.AdicionaJogo(usuario.Id,jogo.Id);
            //Assert            
            repoMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Once);
        }
    }
}
