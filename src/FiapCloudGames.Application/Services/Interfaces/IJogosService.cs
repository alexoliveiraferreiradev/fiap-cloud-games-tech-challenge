using FiapCloudGames.Application.Dtos.Jogos;
using FiapCloudGames.Application.Dtos.Promocao;
using FiapCloudGames.Domain.Enum;

namespace FiapCloudGames.Application.Services.Interfaces
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
        Task<PagedResult<JogoResponse>> ObtemCatalagoJogoPaginado(int pagina =1,int tamanhoPagina = 10);
        Task<JogoResponse> ObtemJogoPorId(Guid jogoId);
        Task<IEnumerable<JogoResponse>> ObtemTodosJogo();
        Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId);
        Task<PagedResult<JogoResponse>> ObtemPorGeneroPaginacao(GeneroJogo generoJogo, int pagina =1, int tamanhoPagina = 10);
        Task<PagedResult<JogoResponse>> ObtemJogosPromovidosPaginacao(int pagina =1, int tamanhoPagina = 10);
        Task DesativaPromocoesInvalidas();
    }
}
