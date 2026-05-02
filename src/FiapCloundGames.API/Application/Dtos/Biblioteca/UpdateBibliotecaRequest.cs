using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Biblioteca
{
    public record UpdateBibliotecaRequest(string nomeJogo, string descricaoJogo, GeneroJogo generoJogo);
}
