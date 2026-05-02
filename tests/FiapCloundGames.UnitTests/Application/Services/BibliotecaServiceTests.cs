using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
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
        [Fact(DisplayName = "Sucesso ao adicionar jogo na biblioteca - adiciona jogo com sucesso")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoValido_DeveAdicionarComSucesso()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid>();

            //Mock
            var repoMock = new Mock<IBibliotecaRepository>();
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaMock = new Mock<IBibliotecaRepository>();
            var jogoMock = new Mock<IJogoRepository>();
            var service = new BibliotecaService(repoMock.Object, usuarioMock.Object, jogoMock.Object);
            usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i =0;i<=3;i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
                bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(false);
                jogosIds.Add(jogo.Id);
            }
            //Act
            await service.LiberarJogosAposPedido(usuario.Id, jogosIds);
            //Assert            
            repoMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Exactly(jogosIds.Count));
        }
        [Fact(DisplayName = "Falha ao adicionar jogo na biblioteca - usuário já possui um dos jogos")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoRepetido_DeveLancarExcecao()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid> ();

            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaMock = new Mock<IBibliotecaRepository>();
            var jogoMock = new Mock<IJogoRepository>();
            var service = new BibliotecaService(bibliotecaMock.Object, usuarioMock.Object, jogoMock.Object);
            usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
                bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(true);
                jogosIds.Add(jogo.Id);
            }
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.LiberarJogosAposPedido(usuario.Id, jogosIds));
            //Assert
            Assert.Equal(MensagensDominio.BibliotecaJogoRepetido, result.Message);
            bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo na biblioteca - jogo não encontrado")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid> ();
            //Mock
            var usuarioMock = new Mock<IUsuarioRepository>();
            var bibliotecaMock = new Mock<IBibliotecaRepository>();
            var jogoMock = new Mock<IJogoRepository>();
            var service = new BibliotecaService(bibliotecaMock.Object, usuarioMock.Object, jogoMock.Object);
            usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(false);
                jogosIds.Add(jogo.Id);
            }
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.LiberarJogosAposPedido(usuario.Id, jogosIds));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);
            bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Never);
        }
    }
}
