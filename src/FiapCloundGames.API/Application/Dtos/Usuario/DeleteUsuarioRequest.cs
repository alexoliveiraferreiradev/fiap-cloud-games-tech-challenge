using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public record DeleteUsuarioRequest(Guid id, MotivoExclusao motivoDelecao);
}
