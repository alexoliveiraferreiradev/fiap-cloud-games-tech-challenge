using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        public BibliotecaService(IBibliotecaRepository bibliotecaRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;   
        }
        public Task AdicionaJogo(CriaBibliotecaRequest criaBibliotecaRequest)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarDados(UpdateBibliotecaRequest updateBibliotecaRequest)
        {
            throw new NotImplementedException();
        }
    }
}
