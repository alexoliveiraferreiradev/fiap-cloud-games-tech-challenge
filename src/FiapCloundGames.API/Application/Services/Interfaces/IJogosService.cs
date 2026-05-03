using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<Jogo> AdicionaJogo(Jogo jogo);
        Task AtualizarJogo(Guid usuarioId,Jogo novoJogo);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
        Task<bool> VerificaDuplicidadeNome(string nomeJogo);
        Task AdicionarPromocao(Promocao promocao);
        Task AtualizaPromocao(Guid promocaoId,Promocao novaPromocao);
        Task DesativarPromocao(Guid jogoId,Guid promocaoId);
        Task<IEnumerable<JogoResponse>> ObtemCatalagoJogos();
        Task<Jogo> ObtemJogoPorId(Guid jogoId);
    }
}
