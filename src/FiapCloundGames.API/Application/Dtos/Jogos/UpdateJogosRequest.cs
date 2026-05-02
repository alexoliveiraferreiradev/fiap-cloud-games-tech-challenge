using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record UpdateJogosRequest(string novoNome, string novaDescricao, decimal novoPreco, GeneroJogo novoGenero);
}
