using FiapCloudGames.Application.Dtos.Jogos;
using FiapCloudGames.Application.Dtos.Promocao;
using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IJogosService
    {
        Task<JogoResponse> AdicionaJogo(CriarJogoRequest request);
        Task<JogoResponse> AtualizarJogo(Guid jogoId,UpdateJogoRequest updateJogoRequest);
        Task Desativar(Guid jogoId);
        Task Reativar(Guid jogoId);
        Task<bool> VerificaDuplicidadeNome(string nomeJogo);
        Task<PromocaoResponse> AdicionarPromocao(CriaPromocaoRequest criaPromocaoRequest);
        Task AtualizaPromocao(Guid promocaoId,UpdatePromocaoRequest criaPromocaoRequest);
        Task DesativarPromocao(Guid promocaoId);        
        Task<JogoResponse> ObtemJogoPorId(Guid jogoId);
        Task<IEnumerable<JogoResponse>> ObtemTodosJogo();
        Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId);        
        Task DesativaPromocoesInvalidas();
        Task<PagedResult<JogoResponse>> ObtemPaginado(JogoFiltroRequest filtroRequest);
        Task<PagedResult<PromocaoResponse>> ObtemPromocaoPaginado(JogoFiltroRequest filtroRequest);
    }
}
