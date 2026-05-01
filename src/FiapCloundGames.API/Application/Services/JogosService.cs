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
        public async Task<Jogos> CriaJogo(CriarJogoRequest request)
        {
            await VerificaDuplicidadeNome(request.Nome);
            var preco = new Preco(request.preco);
            var nomeJogoVO = new NomeJogo(request.Nome);
            var descricaoVO = new Descricao(request.Descricao);
            var jogos = new Jogos(nomeJogoVO, descricaoVO, preco, request.Genero);
            await Adicionar(jogos);
            return jogos;
        }

        private async Task Adicionar(Jogos jogos)
        {
            await _jogoRepository.Adicionar(jogos);
        }

        public async Task AtualizarJogo(Guid id, UpdateJogosRequest updateJogosRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var preco = new Preco(updateJogosRequest.novoPreco);
            jogo.Atualizar(updateJogosRequest.novoNome, updateJogosRequest.novaDescricao, preco, updateJogosRequest.novoGenero);
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
            jogo.AdicionarPromocao(promocaoRequest.valorPromocao, promocaoRequest.dataFim);
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
                Nome = j.Nome,
                Descricao = j.Descricao,
                PrecoOriginal = j.Preco.Valor,
                PrecoAtual = j.ObterPrecoAtual()
            });
        }

    }
}
