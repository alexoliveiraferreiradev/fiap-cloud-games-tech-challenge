using FiapCloudGames.Application.Dtos.Promocao;

namespace FiapCloudGames.Domain.Tests.Fixtures
{
    public class PromocaoFixture
    {       
        public PromocaoFixture()
        {
       
        }

        public CriaPromocaoRequest ObtemPromacaoRequest(Guid jogoId)
        {
            return new CriaPromocaoRequest(jogoId, 90.00m, DateTime.UtcNow, DateTime.UtcNow.AddDays(10));
        }
    }
}
