using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Tags("Gerenciamento de Jogos")]
    [Authorize(Roles = "AdminRole")]
    public class JogoController : ControllerBase
    {
        private readonly ILogger<JogoController> _logger;
        private readonly IJogosService _jogoService;
        public JogoController(ILogger<JogoController> logger, IJogosService jogosService)
        {
            _logger = logger;
            _jogoService = jogosService;
        }
        /// <summary>
        /// Recupera os detalhes de um jogo específico através do seu identificador único.
        /// </summary>
        /// <param name="id">O GUID do jogo cadastrado no sistema.</param>
        /// <response code="200">Sucesso. Retorna os dados detalhados do jogo.</response>
        /// <response code="404">Não encontrado. O ID fornecido não existe na base de dados.</response>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoResponse>> ObterJogoPorId(Guid id)
        {
            _logger.LogInformation("Iniciando busca de detalhes do jogo. JogoId: {JogoId}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo is null)
            {
                _logger.LogInformation("Consulta finalizada. Jogo não encontrado no catálogo. JogoId: {JogoId}", id);
                return NotFound();
            }
            _logger.LogInformation("Jogo recuperado com sucesso. JogoId: {JogoId}, Nome: {NomeJogo}", jogo.Id, jogo.Nome);
            return Ok(jogo);
        }
        /// <summary>
        /// Realiza o cadastro de um novo jogo no catálogo do marketplace.
        /// </summary>
        /// <remarks>
        /// * **Validação de Nome do Jogo:** Obrigatório, entre 3 e 100 caracteres.
        /// * **Validação de Descrição do Jogo:** Obrigatório, entre 5 e 500 caracteres.
        /// * **Validação de Preco:** Obrigatório, não sendo negativo.
        /// Exemplo de requisição:
        /// 
        ///     POST /api/jogos
        ///     {
        ///        "nome": "Euro Truck Simulator 2",
        ///        "preco": 49.90,
        ///        "genero": 1
        ///     }
        /// </remarks>
        /// <param name="jogoRequest">Dados necessários para a criação do jogo.</param>
        /// <response code="201">Criado. O jogo foi inserido com sucesso.</response>
        /// <response code="400">Requisição inválida. Verifique os campos obrigatórios e formatos.</response>
        [HttpPost]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<JogoResponse>> Adicionar(CriarJogoRequest jogoRequest)
        {
            _logger.LogInformation("Iniciando criação de novo jogo. Nome: {NomeJogo}, Preco: {PrecoBase}",jogoRequest.Nome, jogoRequest.Preco);
            var jogo = await _jogoService.AdicionaJogo(jogoRequest);
            _logger.LogInformation("Jogo adicionado com sucesso ao catálogo. JogoId: {JogoId}, Nome: {NomeJogo}",jogo.Id, jogo.Nome);
            return CreatedAtAction(nameof(ObterJogoPorId), new { id = jogo.Id }, jogo);
        }

        /// <summary>
        /// Desativa um jogo do catálogo (Exclusão Lógica).
        /// </summary>
        /// <remarks>
        /// O jogo não é removido fisicamente do banco de dados, mas deixa de ser listado no catálogo ativo.
        /// </remarks>
        /// <param name="id">O identificador único do jogo a ser desativado.</param>
        /// <response code="204">Sem conteúdo. O jogo foi desativado com sucesso.</response>
        /// <response code="404">Não encontrado. O jogo informado não existe.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Desativar(Guid id)
        {
            _logger.LogInformation("Iniciando processo de desativação de jogo. JogoId: {JogoId}", id);

            await _jogoService.Desativar(id);

            _logger.LogInformation("Jogo desativado com sucesso e removido da vitrine ativa. JogoId: {JogoId}", id);
            return NoContent();
        }

        /// <summary>
        /// Atualiza as informações de um jogo existente.
        /// * **Validação de Nome do Jogo:** Obrigatório, entre 3 e 100 caracteres.
        /// * **Validação de Descrição do Jogo:** Obrigatório, entre 5 e 500 caracteres.
        /// * **Validação de Preco:** Obrigatório, não sendo negativo.
        /// </summary>
        /// <param name="id">O identificador único do jogo.</param>
        /// <param name="updateRequest">Novos dados para o jogo (Nome, Preço, etc.).</param>
        /// <response code="200">Sucesso. Retorna o objeto do jogo com as informações atualizadas.</response>
        /// <response code="404">Não encontrado. O jogo informado não existe para ser atualizado.</response>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(JogoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<JogoResponse>> Atualizar(Guid id, UpdateJogoRequest updateRequest)
        {
            _logger.LogInformation("Recebida requisição para atualizar jogo. ID: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo == null)
                return NotFound();

            return await _jogoService.AtualizarJogo(id, updateRequest);
        }

        /// <summary>
        /// Reativa um jogo que foi anteriormente desativado no sistema.
        /// </summary>
        /// <remarks>
        /// Esta operação reverte a desativação lógica, tornando o jogo disponível novamente 
        /// para visualização no catálogo e para novas vendas.
        /// </remarks>
        /// <param name="id">O identificador único do jogo a ser reativado.</param>
        /// <response code="200">Sucesso. O jogo foi reativado com sucesso.</response>
        /// <response code="404">Não encontrado. O jogo com o ID fornecido não existe na base de dados.</response>
        [HttpPut("reativar/{id:guid}")] // Rota alterada para evitar conflito com o 'Atualizar'
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Reativar(Guid id, UpdateJogoRequest updateRequest)
        {
            _logger.LogInformation("Recebida requisição para atualizar jogo. ID: {Id}", id);
            var jogo = await _jogoService.ObtemJogoPorId(id);
            if (jogo == null)
                return NotFound();

            await _jogoService.Reativar(id);
            _logger.LogInformation("Jogo {Id} reativado com sucesso.", id);
            return Ok();
        }
    }
}
