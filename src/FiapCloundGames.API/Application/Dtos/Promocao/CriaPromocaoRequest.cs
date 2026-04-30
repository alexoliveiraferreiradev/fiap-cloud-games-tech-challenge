namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public record CriaPromocaoRequest(Guid jogoId, decimal valorPromocao, DateTime dataFim);
}
