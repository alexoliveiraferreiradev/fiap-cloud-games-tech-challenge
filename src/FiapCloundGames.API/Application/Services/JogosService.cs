using AutoMapper;
using Azure;
using Azure.Core;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace FiapCloundGames.API.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;
        private readonly ILogger<JogosService> _logger;
        public JogosService(IJogoRepository jogoRepository, IMapper mapper, IDistributedCache cache, ILogger<JogosService> logger)
        {
            _jogoRepository = jogoRepository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }
        public async Task<JogoResponse> AdicionaJogo(CriarJogoRequest request)
        {
            await VerificaDuplicidadeNome(request.Nome);
            var preco = new Preco(request.Preco);
            var nomeJogoVO = new NomeJogo(request.Nome);
            var descricaoVO = new Descricao(request.Descricao);
            var jogo = new Jogo(nomeJogoVO, descricaoVO, preco, request.Genero);

            await _jogoRepository.Adicionar(jogo);
            await _cache.RemoveAsync("jogos:catalogo:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogos:catalogo:genero:{jogo.Genero}:pagina:1:tamanho:10");
            return _mapper.Map<JogoResponse>(jogo);
        }


        public async Task<JogoResponse> AtualizarJogo(Guid id, UpdateJogoRequest updateJogosRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null) 
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            var precoVO = new Preco(updateJogosRequest.NovoPreco);
            var nomeJogoVO = new NomeJogo(updateJogosRequest.NovoNome);
            var descricaoJogoVO = new Descricao(updateJogosRequest.NovaDescricao);

            jogo.Atualizar(nomeJogoVO, descricaoJogoVO, precoVO, updateJogosRequest.NovoGenero);
            await _jogoRepository.Atualizar(jogo);
            await _cache.RemoveAsync("jogos:catalogo:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogos:catalogo:genero:{jogo.Genero}:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogos:detalhes:{id}");
            return _mapper.Map<JogoResponse>(jogo);
        }

        public async Task Desativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) 
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            jogo.Desativar();
            await _jogoRepository.Atualizar(jogo);
            await _cache.RemoveAsync("jogos:catalogo:pagina:1:tamanho:10");
            await _cache.RemoveAsync("jogos:catalogo:genero:pagina:1:tamanho:10");
            await _cache.RemoveAsync("jogos:promocoes:v1:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogos:detalhes:{jogoId}");
        }

        public async Task Reativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) 
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            jogo.Reativar();
            await _jogoRepository.Atualizar(jogo);
            await _cache.RemoveAsync("jogos:catalogo:pagina:1:tamanho:10");
            await _cache.RemoveAsync("jogos:catalogo:genero:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogos:detalhes:{jogoId}");
        }

        public async Task<bool> VerificaDuplicidadeNome(string nomeJogo)
        {
            var jogo = await _jogoRepository.ObtemPorNome(nomeJogo);
            if (jogo != null) 
                throw new DomainException(MensagensDominio.JogoMesmoNomeExistente);

            return false;
        }

        public async Task AdicionarPromocao(CriaPromocaoRequest promocaoRequest)
        {
            var periodoVO = new Periodo(promocaoRequest.DataInicio, promocaoRequest.DataFim);
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);

            var valorPromocaoVO = new Preco(promocaoRequest.ValorPromocao);

            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            await _jogoRepository.Atualizar(jogo);
            await _cache.RemoveAsync("jogos:promocoes:pagina:1:tamanho:10");
            await _cache.RemoveAsync($"jogo:detalhes:{promocaoRequest.JogoId}");
        }

        public async Task AtualizaPromocao(Guid promocaoId, UpdatePromocaoRequest promocaoRequest)
        {
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
            
            jogo.AlteraPromocao(promocao.Id, new Preco(promocaoRequest.NovoValorPromocao), promocaoRequest.NovaDataFim);
            
            await _jogoRepository.Atualizar(jogo);

            await _cache.RemoveAsync($"promocao:detalhes:{promocaoId}");
            await _cache.RemoveAsync("jogos:promocoes:pagina:1:tamanho:10");
        }

        public async Task DesativarPromocao(Guid promocaoId)
        {
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            
            if (promocao == null) 
                throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            
            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            
            if (jogo == null) 
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            
            jogo.DesativarPromocao(promocaoId);
            
            await _jogoRepository.Atualizar(jogo);
            await _cache.RemoveAsync($"promocao:detalhes:{promocaoId}");
            await _cache.RemoveAsync("jogos:promocoes:pagina:1:tamanho:10");
        }

        public async Task<PagedResult<JogoResponse>> ObtemCatalagoJogoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
            var cacheKey = $"jogos:catalogo:pagina:{pagina}:tamanho:{tamanhoPagina}";
            var dadosCache = await _cache.GetStringAsync(cacheKey);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!string.IsNullOrEmpty(dadosCache))
            {
                _logger.LogInformation("Catálogo recuperado do CACHE. Pagina: {Pagina}", pagina);
                return JsonSerializer.Deserialize<PagedResult<JogoResponse>>(dadosCache, jsonOptions);

            }
            _logger.LogInformation("Cache miss. Buscando catálogo no BANCO DE DADOS. Pagina: {Pagina}", pagina);

            var totalRegistros = (await _jogoRepository.ObtemJogosAtivos()).Count();
            var jogoResponse = _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemCatalogoPaginado(pagina, tamanhoPagina));

            var resultadoPaginado = new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);
            if (jogoResponse.Any())
            {
                var cacheOptios = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(resultadoPaginado, jsonOptions), cacheOptios);
            }

            return new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);
        }
        public async Task<PagedResult<JogoResponse>> ObtemPorGeneroPaginacao(GeneroJogo generoJogo, int pagina = 1, int tamanhoPagina = 10)
        {
            var cacheKey = $"jogos:catalogo:genero:{generoJogo}:pagina:{pagina}:tamanho:{tamanhoPagina}";
            var dadosCache = await _cache.GetStringAsync(cacheKey);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!string.IsNullOrEmpty(dadosCache))
            {
                _logger.LogInformation("Catálogo por gênero recuperado do CACHE. Pagina: {Pagina}", pagina);
                return JsonSerializer.Deserialize<PagedResult<JogoResponse>>(dadosCache, jsonOptions);

            }
            _logger.LogInformation("Cache miss. Buscando catálogo no BANCO DE DADOS. Pagina: {Pagina}", pagina);

            var totalRegistros = (await _jogoRepository.TotalJogoPorGenero(generoJogo));
            var jogoResponse = _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemPorGeneroPaginado(generoJogo, pagina, tamanhoPagina));

            var resultadoPaginado = new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);

            if (jogoResponse.Any())
            {
                var cacheOptios = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(resultadoPaginado, jsonOptions), cacheOptios);
            }

            return resultadoPaginado;
        }
        public async Task<JogoResponse> ObtemJogoPorId(Guid jogoId)
        {
            var cacheKey = $"jogo:detalhes:{jogoId}";
            var dadosCache = await _cache.GetStringAsync(cacheKey);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!string.IsNullOrEmpty(dadosCache))
            {
                return JsonSerializer.Deserialize<JogoResponse>(dadosCache);
            }
            var response = _mapper.Map<JogoResponse>(await _jogoRepository.ObterPorId(jogoId));
            if (response != null)
            {
                var cacheOptios = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };
                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(response,jsonOptions), cacheOptios);
            }
            return response;
        }
        public async Task<PagedResult<JogoResponse>> ObtemJogosPromovidosPaginacao(int pagina = 1, int tamanhoPagina = 10)
        {
            var cacheKey = $"jogos:promocoes:pagina:{pagina}:tamanho:{tamanhoPagina}";
            var dadosCache = await _cache.GetStringAsync(cacheKey);

            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            if (!string.IsNullOrEmpty(dadosCache))
            {
                _logger.LogInformation("Vitrine de PROMOÇÕES recuperada do CACHE. Pagina: {Pagina}", pagina);
                return JsonSerializer.Deserialize<PagedResult<JogoResponse>>(dadosCache, jsonOptions);
            }
            _logger.LogInformation("Cache miss. Buscando PROMOÇÕES no BANCO DE DADOS. Pagina: {Pagina}", pagina);
            var totalRegistros = await _jogoRepository.TotalJogosPromovidos();
            var jogoResponse = _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemJogosPromovidosPaginacao(pagina, tamanhoPagina));

            var resultadoPaginado = new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);
            if (jogoResponse.Any())
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(resultadoPaginado, jsonOptions), cacheOptions);
            }
            return new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);
        }
        public async Task DesativaPromocoesInvalidas()
        {
            await _jogoRepository.DesativaPromocoesInvalidas();
            await _cache.RemoveAsync("jogos:promocoes:v1:pagina:1:tamanho:10");
        }

        public async Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId)
        {
            var cacheKey = $"promocao:detalhes:{promocaoId}";
            var dadosCache = await _cache.GetStringAsync(cacheKey);

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!string.IsNullOrEmpty(dadosCache))
            {
                return JsonSerializer.Deserialize<PromocaoResponse>(dadosCache);
            }
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) 
                throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            
            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            if (jogo == null)
                throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            
            var promocaResponse = _mapper.Map<PromocaoResponse>(promocao);
            _mapper.Map(jogo, promocaResponse);

            if (promocaResponse != null)
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(promocaResponse,jsonOptions), cacheOptions);
            }

            return promocaResponse;
        }
    }
}
