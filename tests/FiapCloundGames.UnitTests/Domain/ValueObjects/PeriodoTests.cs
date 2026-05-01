using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Domain.ValueObjects
{
    public class PeriodoTests
    {
        [Fact(DisplayName = "Sucesso ao criar período - deve criar com sucesso")]
        [Trait("Categoria","Periodo Tests")]
        public void CriaPeriodo_PeriodoValido_DeveCriarComSucesso()
        {
            //Arrange
            var dataFim = DateTime.UtcNow.AddDays(10);
            //Assert
            var periodoVO = new Periodo(dataFim);
            //Act
            Assert.True(periodoVO.DataFim> periodoVO.DataInicio);
            Assert.Equal(dataFim, periodoVO.DataFim);
        }

        [Fact(DisplayName = "Falha ao criar período - data final menor")]
        [Trait("Categoria","Periodo Tests")]
        public void CriaPeriodo_PeriodoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var dataFim = DateTime.UtcNow.AddDays(-10);
            //Assert
            var result = Assert.Throws<DomainException>(() => new Periodo(dataFim));
            //Act
            Assert.Equal(MensagensDominio.DataFimInvalida, result.Message);
        }

    }
}
