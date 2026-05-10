using AutoMapper;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Mappings;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Application.Tests.Fixtures;
using FiapCloudGames.Domain.Common.Exceptions;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Domain.Resources;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FiapCloudGames.Application.Tests
{
    public class BibliotecaServiceTests
    {
        private readonly JogosFixture _jogoFixture;
        private readonly UsuarioFixture _usuarioFixture;
        private readonly Mock<IBibliotecaRepository> _bibliotecaMock;
        private readonly Mock<IUsuarioRepository> _usuarioMock;
        private readonly Mock<IJogoRepository> _jogoMock;
        private IMapper _mapper;
        private readonly ILogger<BibliotecaService> _logger;
        private readonly BibliotecaService _service;
        private readonly Mock<ICacheService> _cacheService;
        public BibliotecaServiceTests()
        {
            _jogoFixture = new JogosFixture();
            _usuarioFixture = new UsuarioFixture();
            _cacheService = new Mock<ICacheService>();  
            _bibliotecaMock = new Mock<IBibliotecaRepository>();
            _usuarioMock = new Mock<IUsuarioRepository>();
            _jogoMock = new Mock<IJogoRepository>();
            var congiMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<BibliotecaProfile>();
            });
            _mapper = congiMapper.CreateMapper();
            _logger = NullLogger<BibliotecaService>.Instance;

            _service = new BibliotecaService(_bibliotecaMock.Object, _usuarioMock.Object,
                _jogoMock.Object, _mapper, _logger, _cacheService.Object);

        }
        [Fact(DisplayName = "Sucesso ao adicionar jogo na biblioteca - adiciona jogo com sucesso")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoValido_DeveAdicionarComSucesso()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid>();

            //Mock
            _usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i =0;i<=3;i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                _jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
                _bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(false);
                jogosIds.Add(jogo.Id);
            }
            //Act
            await _service.LiberarJogosAposPedido(usuario.Id, jogosIds);
            //Assert            
            _bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Exactly(jogosIds.Count));
        }
        [Fact(DisplayName = "Falha ao adicionar jogo na biblioteca - usuário já possui um dos jogos")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoRepetido_DeveLancarExcecao()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid> ();

            //Mock
            _usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                _jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
                _bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(true);
                jogosIds.Add(jogo.Id);
            }
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.LiberarJogosAposPedido(usuario.Id, jogosIds));
            //Assert
            Assert.Equal(MensagensDominio.BibliotecaJogoRepetido, result.Message);
            _bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao adicionar jogo na biblioteca - jogo não encontrado")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_JogoNaoEncontrado_DeveLancarExcecao()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid> ();
            //Mock
            _usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                _bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(false);
                jogosIds.Add(jogo.Id);
            }
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.LiberarJogosAposPedido(usuario.Id, jogosIds));
            //Assert
            Assert.Equal(MensagensDominio.JogoNaoEncontrado, result.Message);
            _bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao adicionar jogo na biblioteca - ja possui o jogo")]
        [Trait("Categoria", "Biblioteca Services")]
        public async Task BibliotecaJogo_VerificaSePossuiJogo_DeveLancarExcecao()
        {
            //Arrage
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var jogosIds = new List<Guid> ();
            //Mock
            _usuarioMock.Setup(u => u.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            for (int i = 0; i <= 3; i++)
            {
                var jogo = _jogoFixture.ObtemJogosComSucesso();
                _jogoMock.Setup(j => j.ObterPorId(jogo.Id)).ReturnsAsync(jogo);
                _bibliotecaMock.Setup(b => b.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id)).ReturnsAsync(true);
                jogosIds.Add(jogo.Id);
            }
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.LiberarJogosAposPedido(usuario.Id, jogosIds));
            //Assert
            Assert.Equal(MensagensDominio.BibliotecaJogoRepetido, result.Message);
            _bibliotecaMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Never);
        }
    }
}
