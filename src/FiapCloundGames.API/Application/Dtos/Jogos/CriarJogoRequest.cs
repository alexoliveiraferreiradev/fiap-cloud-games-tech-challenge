using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record CriarJogoRequest(string Nome, string Descricao, Preco Preco, GeneroJogo Genero);
}
