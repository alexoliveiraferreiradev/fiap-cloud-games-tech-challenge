namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public record UpdatePromocaoRequest(Guid jogoId,decimal novoValorPromocao, DateTime novaDataFim);
}
