using FiapCloundGames.API.Application.Dtos.Biblioteca;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task AdicionaJogo(CriaBibliotecaRequest criaBibliotecaRequest);
        Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds);
    }
}
