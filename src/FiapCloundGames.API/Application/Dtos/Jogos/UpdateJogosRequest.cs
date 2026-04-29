using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record UpdateJogosRequest(string novoNome, string novaDescricao, decimal novoPreco, GeneroJogo novoGenero);
}
