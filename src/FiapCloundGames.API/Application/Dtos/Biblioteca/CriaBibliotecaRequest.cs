using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Biblioteca
{
    public record CriaBibliotecaRequest(Guid usuarioId, Guid jogoId);
}
