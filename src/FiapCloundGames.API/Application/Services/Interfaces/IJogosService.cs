using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IJogosService
    {
        Task<JogoResponse> AdicionaJogo(CriarJogoRequest request);
        Task<JogoResponse> AtualizarJogo(Guid jogoId,UpdateJogoRequest updateJogoRequest);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
        Task<bool> VerificaDuplicidadeNome(string nomeJogo);
        Task AdicionarPromocao(CriaPromocaoRequest criaPromocaoRequest);
        Task AtualizaPromocao(Guid promocaoId,UpdatePromocaoRequest criaPromocaoRequest);
        Task DesativarPromocao(Guid promocaoId);
        Task<IEnumerable<JogoResponse>> ObtemCatalagoJogos();
        Task<JogoResponse> ObtemJogoPorId(Guid jogoId);
        Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId);
        Task<IEnumerable<JogoResponse>> ObtemPorGenero(GeneroJogo generoJogo);
        Task<IEnumerable<JogoResponse>> ObtemJogosPromovidos();
        Task DesativaPromocoesInvalidas();
    }
}
