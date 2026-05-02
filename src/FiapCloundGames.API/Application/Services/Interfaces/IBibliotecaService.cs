using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task AdicionaJogo(CriaBibliotecaRequest criaBibliotecaRequest);
        Task AtualizarDados(UpdateBibliotecaRequest updateBibliotecaRequest);
    }
}
