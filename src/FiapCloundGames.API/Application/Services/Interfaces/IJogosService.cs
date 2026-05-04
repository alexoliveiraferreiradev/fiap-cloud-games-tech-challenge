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
        Task DesativarPromocao(Guid promocaoId);
        Task<IEnumerable<Jogo>> ObtemCatalagoJogos();
        Task<Jogo> ObtemJogoPorId(Guid jogoId);
        Task<Promocao?> ObtemPromocaoPorId(Guid promocaoId);
        Task<IEnumerable<Jogo>> ObtemPorGenero(GeneroJogo generoJogo);
        Task<IEnumerable<Jogo>> ObtemJogosPromovidos();
        Task DesativaPromocoesInvalidas();
    }
}
