using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task Adiciona(NomeJogo nomeJogo, Descricao descricao, GeneroJogo generoJogo);
        Task AtualizaDados(NomeJogo nomeJogo, Descricao descricao, GeneroJogo generoJogo);
        Task Limpar();
    }
}
