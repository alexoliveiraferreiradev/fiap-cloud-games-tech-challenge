using FiapCloudGames.Application.Dtos.Biblioteca;
using FiapCloudGames.Domain.Common;

namespace FiapCloudGames.Application.Interfaces
{
    public interface IBibliotecaService
    {
        Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<PagedResult<BibliotecaResponse>> ObtemBibliotecaDoUsuarioPaginacao(Guid usuarioId, int pagina =1, int tamanhoPagina = 10);
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId);
    }
}
