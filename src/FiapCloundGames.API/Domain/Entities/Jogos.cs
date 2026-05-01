using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Jogos : AgreggateRoot
    {
        public NomeJogo Nome { get; private set; }
        public Descricao Descricao { get; private set; }
        public Preco PrecoBase { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public GeneroJogo Genero { get; private set; }
        private List<Promocao> _promocoes = new List<Promocao>();
        public IReadOnlyCollection<Promocao> Promocoes => _promocoes;

        protected Jogos()
        {
        }


        public Jogos(NomeJogo nomeJogo, Descricao descricaoJogo, Preco precoJogo, GeneroJogo generoJogo)
        {
            Nome = nomeJogo;
            Descricao = descricaoJogo;
            PrecoBase = precoJogo;
            Genero = generoJogo;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentRange((int)Genero, 1, 15, MensagensDominio.JogoGeneroObrigatorio);
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
        }

        public void Desativar()
        {
            AssertionConcern.AssertStateFalse(Ativo, MensagensDominio.JogoInvalido);
            Ativo = false;
            DataAlteracao = DateTime.UtcNow;
        }

        public void Reativar()
        {
            if (Ativo) throw new DomainException(MensagensDominio.JogoAtivo);
            Ativo = true;
            DataAlteracao = DateTime.UtcNow;
        }

        public void Atualizar(NomeJogo novoNome, Descricao novaDescricao, Preco novoPreco, GeneroJogo novoGenero)
        {
            AssertionConcern.AssertStateFalse(Ativo, MensagensDominio.JogoInvalido);

            AtualizarNome(novoNome);
            AtualizarDescricao(novaDescricao);
            AtualizarPreco(novoPreco);
            AtualizarGenero(novoGenero);
            DataAlteracao = DateTime.UtcNow;
        }

        private void AtualizarGenero(GeneroJogo novoGenero)
        {
            AssertionConcern.AssertArgumentRange((int)Genero, 1, 15, MensagensDominio.JogoGeneroObrigatorio);
            if (Genero == novoGenero) return;
            Genero = novoGenero;
        }

        private void AtualizarPreco(Preco novoPreco)
        {
            if (PrecoBase == novoPreco) return;
            PrecoBase = novoPreco;
        }

        private void AtualizarDescricao(Descricao novaDescricao)
        {
            AssertionConcern.AssertArgumentNotNull(novaDescricao, MensagensDominio.JogoDescricaoObrigatoria);
            if (Descricao == novaDescricao) return;
            Descricao = novaDescricao;
        }

        private void AtualizarNome(NomeJogo novoNome)
        {
            AssertionConcern.AssertArgumentNotNull(novoNome, MensagensDominio.JogoNomeObrigatorio);
            if (Nome == novoNome) return;
            Nome = novoNome;
        }

        public void AdicionarPromocao(Preco valorPromocao, DateTime dataFim)
        {
            if (valorPromocao.Valor >= PrecoBase.Valor) throw new DomainException(MensagensDominio.PromocaoValorMaior);
            foreach (var p in _promocoes.Where(x => x.Ativo)) p.Desativar();
            _promocoes.Add(new Promocao(Id, valorPromocao, dataFim));
        }
        public decimal ObterPrecoAtual()
        {
            var promoAtiva = _promocoes.FirstOrDefault(p => p.EstaValida());
            return promoAtiva != null ? promoAtiva.ValorPromocao : PrecoBase.Valor;
        }
        public void DesativarPromocao(Guid promocaoId)
        {
            var promocao = _promocoes.FirstOrDefault(x => x.Id == promocaoId);
            if (promocao == null) throw new DomainException(MensagensDominio.PromocaoNaoEncontrada);
            promocao.Desativar();   

        }
    }
}
