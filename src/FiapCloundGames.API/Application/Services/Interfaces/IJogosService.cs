using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<Jogos> CriaJogo(CriarJogoRequest request);
        Task AtualizarJogo(Guid usuarioId,UpdateJogosRequest updateJogoRequest);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
    }
}
