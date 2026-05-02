using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Biblioteca : AggregateRoot
    {
        public virtual Usuario Usuario { get; private set; }
        public Guid UsuarioId { get; private set; }
        public virtual ICollection<Jogo> Jogos { get; private set; }
        public Guid JogoId { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataAlteracao { get; private set; }
        public bool Ativo { get; private set; }

        public Biblioteca(Guid usuarioId, Guid jogoId)
        {
            UsuarioId = usuarioId;
            JogoId = jogoId;
            DataCadastro = DateTime.UtcNow;
            DataAlteracao = DataCadastro;
            Ativo = true;
            ValidarEntidade();
        }

        protected override void ValidarEntidade()
        {
            AssertionConcern.AssertArgumentNotEquals(UsuarioId,Guid.Empty, MensagensDominio.UsuarioNaoEncontrado);
            AssertionConcern.AssertArgumentNotEquals(JogoId,Guid.Empty, MensagensDominio.JogoNaoEncontrado);
        }
       
    }
}
