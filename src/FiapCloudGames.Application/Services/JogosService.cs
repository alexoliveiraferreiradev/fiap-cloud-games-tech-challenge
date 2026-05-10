using AutoMapper;
using FiapCloudGames.Application.Dtos.Jogos;
using FiapCloudGames.Application.Dtos.Promocao;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Common.Exceptions;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Domain.Resources;
using FiapCloudGames.Domain.ValueObjects;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FiapCloudGames.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly ILogger<JogosService> _logger;
        public JogosService(IJogoRepository jogoRepository, IMapper mapper, ICacheService cacheService, ILogger<JogosService> logger)
        {
            _jogoRepository = jogoRepository;
            _mapper = mapper;
            _cacheService = cacheService;
            _logger = logger;
        }
        public async Task<JogoResponse> AdicionaJogo(CriarJogoRequest request)
        {
            _logger.LogInformation("Iniciando o cadastro de um novo jogo. Nome: {NomeJogo}, Gênero: {Genero}", request.Nome, request.Genero);

            await VerificaDuplicidadeNome(request.Nome);

            var preco = new Preco(request.Preco);
            var nomeJogoVO = new NomeJogo(request.Nome);
            var descricaoVO = new Descricao(request.Descricao);
            var jogo = new Jogo(nomeJogoVO, descricaoVO, preco, request.Genero);

            await _jogoRepository.Adicionar(jogo);

            _logger.LogInformation("Jogo {JogoId} salvo no banco de dados com sucesso. Invalidando caches de vitrine...", jogo.Id);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Cadastro do jogo {JogoId} concluído e caches atualizados.", jogo.Id);
            return _mapper.Map<JogoResponse>(jogo);
        }


        public async Task<JogoResponse> AtualizarJogo(Guid id, UpdateJogoRequest updateJogosRequest)
        {
            _logger.LogInformation("Iniciando a atualização do jogo {JogoId}.", id);
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            var precoVO = new Preco(updateJogosRequest.NovoPreco);
            var nomeJogoVO = new NomeJogo(updateJogosRequest.NovoNome);
            var descricaoJogoVO = new Descricao(updateJogosRequest.NovaDescricao);

            jogo.Atualizar(nomeJogoVO, descricaoJogoVO, precoVO, updateJogosRequest.NovoGenero);
            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Jogo {JogoId} atualizado no banco de dados. Procedendo com a invalidação dos caches...", id);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverAsync($"jogo:detalhes:{id}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de atualização do jogo {JogoId} finalizado com sucesso.", id);

            return _mapper.Map<JogoResponse>(jogo);
        }

        public async Task Desativar(Guid jogoId)
        {
            _logger.LogInformation("Iniciando a inativação do jogo {JogoId}.", jogoId);
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            jogo.Desativar();
            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Jogo {JogoId} inativado no banco de dados. Invalidando caches das vitrines...", jogoId);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverAsync($"jogo:detalhes:{jogoId}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de inativação do jogo {JogoId} finalizado com sucesso e caches limpos.", jogoId);
        }

        public async Task Reativar(Guid jogoId)
        {
            _logger.LogInformation("Iniciando a reativação do jogo {JogoId}.", jogoId);
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            jogo.Reativar();
            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Jogo {JogoId} reatviado no banco de dados. Invalidando caches das vitrines...", jogoId);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverAsync($"jogo:detalhes:{jogoId}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de reativação do jogo {JogoId} finalizado com sucesso e caches limpos.", jogoId);
        }

        public async Task<bool> VerificaDuplicidadeNome(string nomeJogo)
        {
            var jogo = await _jogoRepository.ObtemPorNome(nomeJogo);
            if (jogo != null)
                throw new DomainException(MensagensDominio.JogoMesmoNomeExistente);

            return false;
        }

        public async Task<PromocaoResponse> AdicionarPromocao(CriaPromocaoRequest promocaoRequest)
        {
            _logger.LogInformation("Iniciando a adição de promoção para o jogo {JogoId}.", promocaoRequest.JogoId);
            var periodoVO = new Periodo(promocaoRequest.DataInicio, promocaoRequest.DataFim);
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            if (jogo.Promocoes.Any())
                throw new DomainException(MensagensDominio.JogoPromocoes);

            var valorPromocaoVO = new Preco(promocaoRequest.ValorPromocao);

            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Promoção adicionada ao jogo {JogoId} no banco de dados. Invalidando caches...", promocaoRequest.JogoId);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverAsync($"jogo:detalhes:{promocaoRequest.JogoId}");
            await _cacheService.RemoverAsync($"promocao:detalhes:{jogo.Promocoes.Select(x => x.Id).First()}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de adição de promoção ao jogo {JogoId} concluído com sucesso.", promocaoRequest.JogoId);

            var novaPromocao = jogo.Promocoes.First();

            var response = _mapper.Map<PromocaoResponse>(jogo);

            return _mapper.Map(novaPromocao, response);
        }

        public async Task AtualizaPromocao(Guid promocaoId, UpdatePromocaoRequest promocaoRequest)
        {
            _logger.LogInformation("Iniciando atualização da promoção {PromocaoId} vinculada ao jogo {JogoId}.", promocaoId, promocaoRequest.JogoId);
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.JogoId);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            var novoPrecoPromocao = new Preco(promocaoRequest.NovoValorPromocao);
            var novaDataPromocao = new Periodo(promocaoRequest.NovaDataFim);

            if (!jogo.Promocoes.Any())
                throw new DomainException(MensagensDominio.JogoSemPromocoes);

            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);

            if (promocao == null)
                throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);

            jogo.AlteraPromocao(promocao.Id, novoPrecoPromocao, promocaoRequest.NovaDataFim);

            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Promoção {PromocaoId} atualizada no banco. Invalidando caches...", promocaoId);

            await _cacheService.RemoverAsync($"jogos:detalhes:{promocaoRequest.JogoId}");
            await _cacheService.RemoverAsync($"promocao:detalhes:{promocaoId}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Atualização da promoção {PromocaoId} concluída com sucesso.", promocaoId);
        }

        public async Task DesativarPromocao(Guid promocaoId)
        {
            _logger.LogInformation("Iniciando a inativação da promoção {PromocaoId}.", promocaoId);

            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);

            if (promocao == null)
                throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);

            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);

            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            jogo.DesativarPromocao(promocaoId);

            await _jogoRepository.Atualizar(jogo);

            _logger.LogInformation("Promoção {PromocaoId} inativada no banco de dados. Invalidando caches...", promocaoId);

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverAsync($"promocao:detalhes:{promocaoId}");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de inativação da promoção {PromocaoId} finalizado com sucesso.", promocaoId);
        }

        public async Task<PagedResult<PromocaoResponse>> ObtemPromocaoPaginado(JogoFiltroRequest filtro)
        {           
            string p = filtro.ApenasPromovidos.GetValueOrDefault() ? "sim" : "nao";

            var cacheKey = $"jogos:pag:p{filtro.Pagina}:t{filtro.Tamanho}:prom_{p}";

            var jogosEmCache = await _cacheService.ObterAsync<PagedResult<PromocaoResponse>>(cacheKey);

            if (jogosEmCache != null)
            {
                _logger.LogInformation("Catálogo recuperado do CACHE. Pagina: {Pagina}", filtro.Pagina);
                return jogosEmCache;

            }
            _logger.LogInformation("Cache miss. Buscando catálogo no BANCO DE DADOS. Pagina: {Pagina}", filtro.Pagina);

            var pagedJogos = await _jogoRepository.ObtemPaginado(filtro.Pagina, filtro.Tamanho, filtro.Busca, filtro.Genero, filtro.ApenasPromovidos);
            
            var promocaoMapeado = _mapper.Map<List<PromocaoResponse>>(pagedJogos.Itens);            

            if (pagedJogos.Itens.Any())
            {
                await _cacheService.DefinirAsync(cacheKey, promocaoMapeado, TimeSpan.FromMinutes(5));
            }

            return new PagedResult<PromocaoResponse>(promocaoMapeado, pagedJogos.PageNumber, pagedJogos.TotalPages, pagedJogos.TotalItens);
        }

        public async Task<PagedResult<JogoResponse>> ObtemPaginado(JogoFiltroRequest filtro)
        {
            string b = string.IsNullOrWhiteSpace(filtro.Busca) ? "todos" : filtro.Busca.ToLower().Trim();
            string g = filtro.Genero.HasValue ? filtro.Genero.Value.ToString() : "todos";
            string p = filtro.ApenasPromovidos.GetValueOrDefault() ? "sim" : "nao";

            var cacheKey = $"jogos:pag:p{filtro.Pagina}:t{filtro.Tamanho}:b_{b}:g_{g}:prom_{p}";

            var jogosEmCache = await _cacheService.ObterAsync<PagedResult<JogoResponse>>(cacheKey);

            if (jogosEmCache != null)
            {
                _logger.LogInformation("Catálogo recuperado do CACHE. Pagina: {Pagina}", filtro.Pagina);
                return jogosEmCache;

            }
            _logger.LogInformation("Cache miss. Buscando catálogo no BANCO DE DADOS. Pagina: {Pagina}", filtro.Pagina);

            var pagedJogos = await _jogoRepository.ObtemPaginado(filtro.Pagina, filtro.Tamanho, filtro.Busca, filtro.Genero, filtro.ApenasPromovidos);

            var jogoMapeados = _mapper.Map<List<JogoResponse>>(pagedJogos.Itens.ToList());

            if (pagedJogos.Itens.Any())
            {
                await _cacheService.DefinirAsync(cacheKey, JsonSerializer.Serialize(jogoMapeados), TimeSpan.FromMinutes(5));
            }

            return new PagedResult<JogoResponse>(jogoMapeados, pagedJogos.PageNumber, pagedJogos.TotalPages, pagedJogos.TotalItens);
        }


        public async Task<JogoResponse> ObtemJogoPorId(Guid jogoId)
        {
            var cacheKey = $"jogo:detalhes:{jogoId}";
            var jogosEmCache = await _cacheService.ObterAsync<JogoResponse>(cacheKey);

            if (jogosEmCache != null)
            {
                return jogosEmCache;
            }
            var response = _mapper.Map<JogoResponse>(await _jogoRepository.ObterPorId(jogoId));
            if (response != null)
            {
                await _cacheService.DefinirAsync(cacheKey, response, TimeSpan.FromMinutes(5));
            }
            return response;
        }

        public async Task DesativaPromocoesInvalidas()
        {
            _logger.LogInformation("Iniciando varredura em lote para desativar promoções expiradas no banco de dados.");

            await _jogoRepository.DesativaPromocoesInvalidas();

            _logger.LogInformation("Varredura concluída. Invalidando cache da vitrine principal de promoções...");

            await _cacheService.RemoverAsync("jogos:todos");
            await _cacheService.RemoverPorPrefixoAsync("jogos:pag:");

            _logger.LogInformation("Processo de inativação de promoções expiradas finalizado com sucesso.");
        }

        public async Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId)
        {
            var cacheKey = $"promocao:detalhes:{promocaoId}";
            var promocaoEmCache = await _cacheService.ObterAsync<PromocaoResponse>(cacheKey);

            if (promocaoEmCache != null)
            {
                return promocaoEmCache;
            }
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null)
                throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);

            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);


            var promocaResponse = _mapper.Map<PromocaoResponse>(jogo);
            _mapper.Map(promocao, promocaResponse);

            if (promocaResponse != null)
            {
                await _cacheService.DefinirAsync(cacheKey, promocaResponse, TimeSpan.FromMinutes(5));
            }

            return promocaResponse;
        }

        public async Task<IEnumerable<JogoResponse>> ObtemTodosJogo()
        {
            var cacheKey = "jogos:todos";
            var jogoEmCache = await _cacheService.ObterAsync<IEnumerable<JogoResponse>>(cacheKey);

            
            if (jogoEmCache != null)
            {
                return jogoEmCache;
            }

            var jogos = await _jogoRepository.ObterTodos();
            if (jogos == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            var response = _mapper.Map<IEnumerable<JogoResponse>>(jogos);

            if (response != null)
            {
                await _cacheService.DefinirAsync(cacheKey, response, TimeSpan.FromMinutes(5));
            }

            return response;
        }



    }
}
