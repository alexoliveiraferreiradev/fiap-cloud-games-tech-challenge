using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record CriarJogoRequest(string Nome, string Descricao, decimal Preco, GeneroJogo Genero);
}
