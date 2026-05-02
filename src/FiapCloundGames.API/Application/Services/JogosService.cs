using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogosRepository _jogoRepository;
        public JogosService(IJogosRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }
        public async Task<Jogo> CriaJogo(CriarJogoRequest request)
        {
            await VerificaDuplicidadeNome(request.Nome);
            var preco = new Preco(request.preco);
            var nomeJogoVO = new NomeJogo(request.Nome);
            var descricaoVO = new Descricao(request.Descricao);
            var jogos = new Jogo(nomeJogoVO, descricaoVO, preco, request.Genero);
            await Adicionar(jogos);
            return jogos;
        }

        private async Task Adicionar(Jogo jogos)
        {
            await _jogoRepository.Adicionar(jogos);
        }

        public async Task AtualizarJogo(Guid id, UpdateJogosRequest updateJogosRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var precoVO = new Preco(updateJogosRequest.novoPreco);
            var nomeJogoVO = new NomeJogo(updateJogosRequest.novoNome);
            var descricaoJogoVO = new Descricao(updateJogosRequest.novaDescricao);

            jogo.Atualizar(nomeJogoVO, descricaoJogoVO, precoVO, updateJogosRequest.novoGenero);
            await _jogoRepository.Atualizar(jogo);
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

            return true;
        }

        public async Task AdicionarPromocao(CriaPromocaoRequest promocaoRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var valorPromocaoVO = new Preco(promocaoRequest.valorPromocao);
            var periodoVO = new Periodo(promocaoRequest.dataFim);
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task AtualizaPromocao(Guid promocaoId,UpdatePromocaoRequest promocaoRequest)
        {            
            var jogo = await _jogoRepository.ObterPorId(promocaoRequest.jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var novoPrecoPromocao = new Preco(promocaoRequest.novoValorPromocao);
            var novaDataPromocao = new Periodo(promocaoRequest.novaDataFim);
            if (!jogo.Promocoes.Any()) throw new DomainException(MensagensDominio.JogoSemPromocoes);
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);                        
            jogo.AlteraPromocao(promocao.Id, new Preco(promocaoRequest.novoValorPromocao), promocaoRequest.novaDataFim);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task DesativarPromocao(Guid jogoId, Guid promocaoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.DesativarPromocao(promocaoId);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task<IEnumerable<JogoResponse>> ObtemCatalagoJogos()
        {
            var jogos = await _jogoRepository.ObtemJogosAtivos();
            return jogos.Select(j => new JogoResponse
            {
                Id = j.Id,
                Nome = j.Nome.Valor,
                Descricao = j.Descricao.Valor,
                PrecoOriginal = j.PrecoBase.Valor,
                PrecoAtual = j.ObterPrecoAtual().Valor
            });
        }

        public async Task<Jogo> ObtemJogoPorId(Guid jogoId)
        {
            return await _jogoRepository.ObterPorId(jogoId);
        }
    }
}
