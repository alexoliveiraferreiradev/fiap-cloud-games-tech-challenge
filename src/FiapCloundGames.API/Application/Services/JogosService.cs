using AutoMapper;
using Azure;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IMapper _mapper;
        public JogosService(IJogoRepository jogoRepository, IMapper mapper)
        {
            _jogoRepository = jogoRepository;
            _mapper = mapper;
        }
        public async Task<JogoResponse> AdicionaJogo(CriarJogoRequest request)
        {
            await VerificaDuplicidadeNome(request.Nome);
            var preco = new Preco(request.Preco);
            var nomeJogoVO = new NomeJogo(request.Nome);
            var descricaoVO = new Descricao(request.Descricao);
            var jogos = new Jogo(nomeJogoVO, descricaoVO, preco, request.Genero);
            await _jogoRepository.Adicionar(jogos);
            return _mapper.Map<JogoResponse>(jogos);
        }


        public async Task<JogoResponse> AtualizarJogo(Guid id, UpdateJogoRequest updateJogosRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var precoVO = new Preco(updateJogosRequest.NovoPreco);
            var nomeJogoVO = new NomeJogo(updateJogosRequest.NovoNome);
            var descricaoJogoVO = new Descricao(updateJogosRequest.NovaDescricao);
            jogo.Atualizar(nomeJogoVO, descricaoJogoVO, precoVO, updateJogosRequest.NovoGenero);
            await _jogoRepository.Atualizar(jogo);
            return _mapper.Map<JogoResponse>(jogo);
        }

        public async Task Desativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.Desativar();
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task Reativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.Reativar();
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task<bool> VerificaDuplicidadeNome(string nomeJogo)
        {
            var jogo = await _jogoRepository.ObtemPorNome(nomeJogo);
            if (jogo != null) throw new DomainException(MensagensDominio.JogoMesmoNomeExistente);

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
        }

        public async Task AtualizaPromocao(Guid promocaoId, UpdatePromocaoRequest promocaoRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var novoPrecoPromocao = new Preco(promocaoRequest.NovoValorPromocao);
            var novaDataPromocao = new Periodo(promocaoRequest.NovaDataFim);
            if (!jogo.Promocoes.Any()) throw new DomainException(MensagensDominio.JogoSemPromocoes);
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            jogo.AlteraPromocao(promocao.Id, new Preco(promocaoRequest.NovoValorPromocao), promocaoRequest.NovaDataFim);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task DesativarPromocao(Guid promocaoId)
        {
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.DesativarPromocao(promocaoId);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task<PagedResult<JogoResponse>> ObtemCatalagoJogoPaginado(int pagina = 1, int tamanhoPagina = 10)
        {
            var totalRegistros = (await _jogoRepository.ObtemJogosAtivos()).Count();
            var jogoResponse = _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemCatalogoPaginado());
            return new PagedResult<JogoResponse>(jogoResponse, pagina, tamanhoPagina, totalRegistros);
        }
        public async Task<IEnumerable<JogoResponse>> ObtemPorGenero(GeneroJogo generoJogo)
        {
            return _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemPorGenero(generoJogo));
        }
        public async Task<JogoResponse> ObtemJogoPorId(Guid jogoId)
        {
            return _mapper.Map<JogoResponse>(await _jogoRepository.ObterPorId(jogoId));
        }
        public async Task<IEnumerable<JogoResponse>> ObtemJogosPromovidos()
        {
            return _mapper.Map<IEnumerable<JogoResponse>>(await _jogoRepository.ObtemJogosPromovidos());
        }
        public async Task DesativaPromocoesInvalidas()
        {
            await _jogoRepository.DesativaPromocoesInvalidas();
        }

        public async Task<PromocaoResponse?> ObtemPromocaoPorId(Guid promocaoId)
        {
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var promocaResponse = _mapper.Map<PromocaoResponse>(promocao);
            _mapper.Map(jogo, promocaResponse);
            return promocaResponse;
        }
    }
}
