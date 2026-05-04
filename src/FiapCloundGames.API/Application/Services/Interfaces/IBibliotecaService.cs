using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Application.Services.Interfaces
{
    public interface IBibliotecaService
    {
        Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds);
        Task<PagedResult<BibliotecaResponse>> ObtemBibliotecaDoUsuarioPaginacao(Guid usuarioId, int pagina =1, int tamanhoPagina = 10);
        Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId);
        Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId);
    }
}
