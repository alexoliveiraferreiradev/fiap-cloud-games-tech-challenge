using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Jogos : AgreggateRoot
    {
        public string Nome { get; private set; }
        public string Descricao { get; private set; }
        public decimal Preco { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public GeneroJogo Genero { get; private set; }

        protected Jogos()
        {
        }


        public Jogos(string nomeJogo, string descricaoJogo, decimal precoJogo, GeneroJogo generoJogo)
        {
            Nome = nomeJogo;
            Descricao = descricaoJogo;
            Preco = precoJogo;
            Genero = generoJogo;
            ValidarEntidade();
        }

        public override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentEmpty(Nome, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentEmpty(Descricao, MensagensDominio.JogoDescricaoObrigatoria);
            AssertionConcern.AssertArgumentValueFormat(Preco, MensagensDominio.JogoPrecoInvalido);
            AssertionConcern.AssertArgumentLength(Nome, 3, 20, MensagensDominio.JogoTamanhoNomeInvalido);
            AssertionConcern.AssertArgumentLength(Descricao, 5, 100, MensagensDominio.JogoDescricaoTamanhoInvalido);
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

        public void Atualizar(string novoNome, string novaDescricao, decimal novoPreco, GeneroJogo novoGenero)
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

        private void AtualizarPreco(decimal novoPreco)
        {
            AssertionConcern.AssertArgumentValueFormat(novoPreco, MensagensDominio.JogoPrecoInvalido);
            if (Preco == novoPreco) return;
            Preco = novoPreco;
        }

        private void AtualizarDescricao(string novaDescricao)
        {
            AssertionConcern.AssertArgumentEmpty(novaDescricao, MensagensDominio.JogoDescricaoObrigatoria);
            if (Descricao == novaDescricao) return;
            AssertionConcern.AssertArgumentLength(novaDescricao, 5, 100, MensagensDominio.JogoDescricaoTamanhoInvalido);
            Descricao = novaDescricao;
        }

        private void AtualizarNome(string novoNome)
        {
            AssertionConcern.AssertArgumentEmpty(novoNome, MensagensDominio.JogoNomeObrigatorio);
            if (Nome == novoNome) return;
            AssertionConcern.AssertArgumentLength(novoNome, 3, 20, MensagensDominio.JogoTamanhoNomeInvalido);
            Nome = novoNome;
        }
    }
}
