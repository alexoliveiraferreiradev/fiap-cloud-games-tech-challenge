using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<IEnumerable<Biblioteca>> ObterJogosPorUsuario(Guid usuarioId);
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId);
    }
}
