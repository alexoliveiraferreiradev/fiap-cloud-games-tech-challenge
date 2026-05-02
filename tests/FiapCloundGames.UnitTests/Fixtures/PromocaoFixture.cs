using FiapCloundGames.API.Application.Dtos.Promocao;

namespace FiapCloundGames.UnitTests.Fixtures
{
    public class PromocaoFixture
    {       
        public PromocaoFixture()
        {
       
        }

        public CriaPromocaoRequest ObtemPromacaoRequest(Guid jogoId)
        {
            return new CriaPromocaoRequest(jogoId, 90.00m, DateTime.UtcNow.AddDays(10));
        }
    }
}
