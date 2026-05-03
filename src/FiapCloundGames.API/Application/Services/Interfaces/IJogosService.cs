using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<Jogo> AdicionaJogo(CriarJogoRequest request);
        Task<Jogo> AtualizarJogo(Guid jogoId,UpdateJogoRequest updateJogoRequest);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
        Task<bool> VerificaDuplicidadeNome(string nomeJogo);
        Task AdicionarPromocao(CriaPromocaoRequest criaPromocaoRequest);
        Task AtualizaPromocao(Guid promocaoId,UpdatePromocaoRequest criaPromocaoRequest);
        Task DesativarPromocao(Guid jogoId,Guid promocaoId);
        Task<IEnumerable<JogoResponse>> ObtemCatalagoJogos();
        Task<Jogo> ObtemJogoPorId(Guid jogoId);
        Task<IEnumerable<JogoResponse>> ObtemPorGenero(GeneroJogo generoJogo);
    }
}
