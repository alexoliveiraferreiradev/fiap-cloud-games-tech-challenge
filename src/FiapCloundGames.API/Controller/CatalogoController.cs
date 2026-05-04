using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    [Tags("Catálogo de Jogos")]
    public class CatalogoController : ControllerBase
    {
        private readonly ILogger<CatalogoController> _logger;
        private readonly IJogosService _jogoService;
        public CatalogoController(ILogger<CatalogoController> logger, 
            IJogosService jogosService)
        {
            _logger = logger;
            _jogoService = jogosService;
        }
        /// <summary>
        /// Recupera os detalhes completos de um jogo específico através do seu identificador único.
        /// </summary>
        /// <remarks>
        /// Este endpoint realiza uma validação interna de promoções expiradas antes de retornar os dados, 
        /// garantindo que os valores de preço visualizados estejam sempre atualizados.
        /// 
        /// Exemplo de requisição:
        /// 
        ///     GET /api/jogos/3fa85f64-5717-4562-b3fc-2c963f66afa6
        /// 
        /// </remarks>
        /// <param name="id">O identificador único (GUID) do jogo no banco de dados.</param>
        /// <returns>Retorna um objeto contendo todas as informações do jogo solicitado.</returns>
        /// <response code="200">Sucesso. O jogo foi encontrado e os dados são retornados.</response>
        /// <response code="400">O formato do identificador fornecido é inválido.</response>
        /// <response code="404">Nenhum jogo foi encontrado com o ID informado.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            _logger.LogInformation("Consultando detalhes do jogo. JogoId: {JogoId}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo is null)
            {
                _logger.LogInformation("Consulta finalizada. Jogo não encontrado na base de dados. JogoId: {JogoId}", id);
                return NotFound();
            }
            _logger.LogInformation("Consulta bem-sucedida. JogoId: {JogoId}, NomeJogo: {NomeJogo}", jogo.Id, jogo.Nome);
            return Ok(jogo);
        }

        /// <summary>
        /// Obtém o catálogo de jogos ativos com suporte a paginação.
        /// </summary>
        /// <remarks>
        /// Antes de retornar a lista, o sistema executa uma rotina interna para desativar promoções que expiraram, 
        /// garantindo a integridade dos preços exibidos.
        /// 
        /// O retorno é encapsulado em um objeto de paginação (PagedResult), que contém metadados como total de itens e páginas.
        /// </remarks>
        /// <param name="pagina">Número da página desejada (Inicia em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de jogos a serem exibidos por página (Máximo sugerido: 50).</param>
        /// <returns>Retorna um envelope contendo a lista de jogos e informações de paginação.</returns>
        /// <response code="200">Sucesso. Retorna o catálogo paginado.</response>
        /// <response code="404">Não foram encontrados jogos para os critérios informados.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ObtemCatalogoDeJogos([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            _logger.LogInformation("Consultando catálogo de jogos. Pagina: {Pagina}, TamanhoPagina: {TamanhoPagina}", pagina, tamanhoPagina);
            var jogos = await _jogoService.ObtemCatalagoJogoPaginado(pagina,tamanhoPagina);
            if (jogos.Items == null || !jogos.Items.Any())
            {
                _logger.LogInformation("Consulta ao catálogo finalizada. Nenhum jogo encontrado na Pagina: {Pagina}", pagina);
                return NotFound("Não foi encontrado jogos");
            }
            _logger.LogInformation("Catálogo recuperado com sucesso. Pagina: {Pagina}, Quantidade de Jogos Retornados: {QuantidadeJogos}", pagina, jogos.Items.Count());
            return Ok(jogos);
        }

        /// <summary>
        /// Lista os jogos do catálogo filtrados por um gênero específico de forma paginada.
        /// </summary>
        /// <remarks>
        /// Este endpoint permite explorar o catálogo por categorias. Antes da listagem, 
        /// o sistema valida a vigência das promoções para exibir os preços corretos.
        /// </remarks>
        /// <param name="genero">O gênero do jogo (ex: 1 para Ação, 2 para RPG, etc.).</param>
        /// <param name="pagina">Número da página desejada (Inicia em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de jogos por página.</param>
        /// <response code="200">Sucesso. Retorna a lista de jogos do gênero solicitado.</response>
        /// <response code="404">Não foram encontrados jogos para o gênero informado.</response>
        [HttpGet("buscar-por-genero/{genero}")]
        [ProducesResponseType(typeof(PagedResult<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<JogoResponse>>> ListarPorGenero(GeneroJogo genero, [FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            var jogos =await _jogoService.ObtemPorGeneroPaginacao(genero);
            if (!jogos.Items.Any())
                return NotFound("Não foi encontrado jogos");

            return Ok(jogos);
        }

        /// <summary>
        /// Recupera de forma paginada apenas os jogos que possuem promoções ativas e válidas.
        /// </summary>
        /// <remarks>
        /// Ideal para a seção de 'Destaques' ou 'Ofertas' da loja. O sistema garante que 
        /// promoções expiradas sejam removidas antes de processar esta lista.
        /// </remarks>
        /// <param name="pagina">Número da página desejada (Inicia em 1).</param>
        /// <param name="tamanhoPagina">Quantidade de jogos em oferta por página.</param>
        /// <response code="200">Sucesso. Retorna a lista de jogos em promoção.</response>
        /// <response code="404">Não existem jogos com promoções ativas no momento.</response>
        [HttpGet("busca-jogos-com-promocao")]
        [ProducesResponseType(typeof(PagedResult<JogoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoResponse>> ObtemJogosComPromocao([FromQuery] int pagina = 1, [FromQuery] int tamanhoPagina = 10)
        {
            await _jogoService.DesativaPromocoesInvalidas();
            _logger.LogInformation("Iniciando busca por jogos em promoção. Pagina: {Pagina}, TamanhoPagina: {TamanhoPagina}", pagina, tamanhoPagina);
            var jogos = await _jogoService.ObtemJogosPromovidosPaginacao();
            if (jogos.Items == null || !jogos.Items.Any())
            {
                _logger.LogInformation("Busca de promoções finalizada. Nenhum jogo em oferta encontrado na Pagina: {Pagina}", pagina);
                return NotFound("Não foi encontrado nenhum jogo com promoções");
            }
            _logger.LogInformation("Jogos em promoção recuperados com sucesso. Pagina: {Pagina}, Quantidade Retornada: {QuantidadeJogos}", pagina, jogos.Items.Count());
            return Ok(jogos);
        }
    }
}
