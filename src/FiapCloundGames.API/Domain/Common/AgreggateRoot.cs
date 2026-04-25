namespace FiapCloundGames.API.Domain.Common
{
    public abstract class AgreggateRoot : EntityBase, IAggregateRoot
    {
        public virtual void ValidarEntidade() { }
    }
}
