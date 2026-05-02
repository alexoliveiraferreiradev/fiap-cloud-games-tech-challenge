using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Biblioteca : AggregateRoot
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public NomeJogo Nome { get;private set; }
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
    }
}
