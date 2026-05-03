using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogoRepository _jogoRepository;
        public JogosService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }
        public async Task<Jogo> AdicionaJogo(Jogo novoJogo)
        {
            await VerificaDuplicidadeNome(novoJogo.Nome.Valor);
            var preco = new Preco(novoJogo.PrecoBase.Valor);
            var nomeJogoVO = new NomeJogo(novoJogo.Nome.Valor);
            var descricaoVO = new Descricao(novoJogo.Descricao.Valor);
            var jogos = new Jogo(nomeJogoVO, descricaoVO, preco, novoJogo.Genero);
            await Adicionar(jogos);
            return jogos;
        }

        private async Task Adicionar(Jogo jogos)
        {
            await _jogoRepository.Adicionar(jogos);
        }

        public async Task AtualizarJogo(Guid id, Jogo novaPromocao)
        {
            var jogo = await _jogoRepository.ObterPorId(id);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var precoVO = new Preco(novaPromocao.PrecoBase.Valor);
            var nomeJogoVO = new NomeJogo(novaPromocao.Nome.Valor);
            var descricaoJogoVO = new Descricao(novaPromocao.Descricao.Valor);

            jogo.Atualizar(nomeJogoVO, descricaoJogoVO, precoVO, novaPromocao.Genero);
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

        public async Task AdicionarPromocao(Promocao promocao)
        {
            var jogo = await _jogoRepository.ObterPorId(promocao.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var valorPromocaoVO = new Preco(promocao.ValorPromocao.Valor);
            var periodoVO = new Periodo(promocao.Periodo.DataFim);
            jogo.AdicionarPromocao(valorPromocaoVO, periodoVO);
            await _jogoRepository.Atualizar(jogo);
        }

        public async Task AtualizaPromocao(Guid promocaoId, Promocao novaPromocao)
        {            
            var jogo = await _jogoRepository.ObterPorId(novaPromocao.JogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            var novoPrecoPromocao = new Preco(novaPromocao.ValorPromocao.Valor);
            var novaDataPromocao = new Periodo(novaPromocao.Periodo.DataFim);
            if (!jogo.Promocoes.Any()) throw new DomainException(MensagensDominio.JogoSemPromocoes);
            var promocao = await _jogoRepository.ObterPromocaoPorId(promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);                        
            jogo.AlteraPromocao(promocao.Id, new Preco(novaPromocao.ValorPromocao.Valor), novaPromocao.Periodo.DataFim);
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
            (
                j.Id,
                j.Nome.Valor,
                j.Descricao.Valor,
                j.PrecoBase.Valor,
                j.ObterPrecoAtual().Valor
            ));
        }

        public async Task<Jogo> ObtemJogoPorId(Guid jogoId)
        {
            return await _jogoRepository.ObterPorId(jogoId);
        }
    }
}
