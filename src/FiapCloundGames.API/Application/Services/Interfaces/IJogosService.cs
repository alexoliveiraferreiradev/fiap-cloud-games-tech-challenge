using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<Jogo> CriaJogo(CriarJogoRequest request);
        Task AtualizarJogo(Guid usuarioId,UpdateJogoRequest updateJogoRequest);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
        Task<bool> VerificaDuplicidadeNome(string nomeJogo);
        Task AdicionarPromocao(CriaPromocaoRequest criaPromocaoRequest);
        Task AtualizaPromocao(Guid promocaoId,UpdatePromocaoRequest criaPromocaoRequest);
        Task DesativarPromocao(Guid jogoId,Guid promocaoId);
        Task<IEnumerable<JogoResponse>> ObtemCatalagoJogos();
        Task<Jogo> ObtemJogoPorId(Guid jogoId);
    }
}
