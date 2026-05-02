using FiapCloundGames.API.Application.Dtos.Biblioteca;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<IEnumerable<BibliotecaResponse>> ObterJogosPorUsuario(Guid usuarioId);
    }
}
