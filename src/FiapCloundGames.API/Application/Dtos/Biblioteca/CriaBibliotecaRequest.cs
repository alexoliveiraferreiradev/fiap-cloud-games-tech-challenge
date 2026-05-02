using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Biblioteca
{
    public record CriaBibliotecaRequest(string nomeJogo, string descricaoJogo, GeneroJogo genero);
}
