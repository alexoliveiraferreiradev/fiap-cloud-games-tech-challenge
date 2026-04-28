using FiapCloundGames.API.Application.Dtos;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<Jogos> CriaJogo(CriarJogoRequest request);
    }
}
