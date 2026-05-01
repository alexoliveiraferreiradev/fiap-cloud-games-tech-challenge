using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Biblioteca : AgreggateRoot
    {
        public Guid UsuarioId { get; private set; }
        public Guid JogoId { get; private set; }
        public DateTime DataAquisicao { get; private set; }
        public bool Ativo { get; private set; }

        public Biblioteca(Guid usuarioId, Guid jogoId)
        {
            if(usuarioId == Guid.Empty)
                throw new DomainException("Usuário inválido", nameof(usuarioId));
            if(jogoId == Guid.Empty)
                throw new DomainException("Jogo inválido", nameof(jogoId));

            UsuarioId = usuarioId;
            JogoId = jogoId;
            DataAquisicao = DateTime.UtcNow;
            Ativo = true;
        }

        protected override void ValidarEntidade()
        {
            throw new NotImplementedException();
        }
    }
}
