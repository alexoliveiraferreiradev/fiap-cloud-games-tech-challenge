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
        public BibliotecaServiceTests()
        {
            _jogoFixture = new JogosFixture();
        }
        [Fact(DisplayName ="Sucesso ao adicionar jogo na biblioteca - adiciona jogo com sucesso")]
        [Trait("Categoria","Biblioteca Services")]
        public async Task AdicionaJogo_JogoValido_DeveAdicionarComSucesso()
        {
            //Arrage
            var jogo = _jogoFixture.ObtemJogosComSucesso();
            var criaJogoBibliotecaRequest = new CriaBibliotecaRequest(jogo.Nome.Valor, jogo.Descricao.Valor, jogo.Genero);
            //Mock
            var repoMock = new Mock<IBibliotecaRepository>();
            var service = new BibliotecaService(repoMock.Object);
            //Act
            await service.AdicionaJogo(criaJogoBibliotecaRequest);
            //Assert
            repoMock.Verify(r => r.Adicionar(It.IsAny<Biblioteca>()), Times.Once);
        }
    }
}
