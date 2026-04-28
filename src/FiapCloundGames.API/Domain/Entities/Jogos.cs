using FiapCloundGames.API.Domain.Common;
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
        public DateTime DataCadastro { get;private set; }
        public GeneroJogo Genero { get; set; }


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
            AssertionConcern.AssertArgumentNotEmpty(Nome, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentNotEmpty(Descricao, MensagensDominio.JogoDescricaoObrigatoria);
            AssertionConcern.AssertArgumentValueFormat(Preco, MensagensDominio.JogoPrecoInvalido);
            AssertionConcern.AssertArgumentLength(Nome, 3, 20, MensagensDominio.JogoTamanhoNomeInvalido);
            AssertionConcern.AssertArgumentLength(Descricao, 5, 100, MensagensDominio.JogoDescricaoTamanhoInvalido);
            AssertionConcern.AssertArgumentRange((int)Genero,1,15, MensagensDominio.JogoGeneroObrigatorio);
            Ativo = true;
            DataCadastro = DateTime.UtcNow;
        }
    }
}
