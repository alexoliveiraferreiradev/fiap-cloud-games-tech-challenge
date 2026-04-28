using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos
{
    public record CriarJogoRequest(string Nome, string Descricao, decimal Preco, GeneroJogo Genero);
}
