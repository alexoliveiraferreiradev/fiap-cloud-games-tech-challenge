using Bogus;
using FiapCloundGames.API.Application.Dtos;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
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

        [Fact(DisplayName = "Adicionar jogo - Deve criar um jogo com sucesso")]
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
    }
}
