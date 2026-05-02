using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task AdicionaJogo(NomeJogo nomeJogo, Descricao descricao, GeneroJogo generoJogo);
        Task AtualizarDados(NomeJogo nomeJogo, Descricao descricao, GeneroJogo generoJogo);
        Task Limpar();
    }
}
