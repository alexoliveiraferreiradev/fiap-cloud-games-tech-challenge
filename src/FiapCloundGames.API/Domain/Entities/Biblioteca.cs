using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Biblioteca : AggregateRoot
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public NomeJogo Nome { get; private set; }
        public Descricao Descricao { get; private set; }
        public GeneroJogo Genero { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public bool Ativo { get; private set; }

        public Biblioteca(Guid usuarioId, Guid jogoId)
        {
            UsuarioId = usuarioId;
            JogoId = jogoId;
            DataCadastro = DateTime.UtcNow;
            Ativo = true;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotNull(UsuarioId, MensagensDominio.UsuarioNaoEncontrado);
            AssertionConcern.AssertArgumentNotNull(JogoId, MensagensDominio.JogoNaoEncontrado);
        }

        public void AdicionaJogo(NomeJogo nomeJogo, Descricao descricaoJogo, GeneroJogo generoJogo)
        {
            if (!Ativo) throw new DomainException(MensagensDominio.BibliotecaInativa);
            AssertionConcern.AssertArgumentNotNull(nomeJogo, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentNotNull(descricaoJogo, MensagensDominio.JogoDescricaoObrigatoria);
            AssertionConcern.AssertArgumentNotNull(generoJogo, MensagensDominio.JogoGeneroObrigatorio);

            Nome = nomeJogo;
            Descricao = descricaoJogo;
            Genero = generoJogo;
        }
        public void AtualizarDadosJogo(NomeJogo nomeJogo, Descricao descricaoJogo, GeneroJogo generoJogo)
        {
            if (!Ativo) throw new DomainException(MensagensDominio.BibliotecaInativa);
            AssertionConcern.AssertArgumentNotNull(nomeJogo, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentNotNull(descricaoJogo, MensagensDominio.JogoDescricaoObrigatoria);
            AssertionConcern.AssertArgumentNotNull(generoJogo, MensagensDominio.JogoGeneroObrigatorio);

            if (Nome != nomeJogo) Nome = nomeJogo;

            if (Descricao != descricaoJogo) Descricao = descricaoJogo;

            if (Genero != generoJogo) Genero = generoJogo;

        }
    }
}
