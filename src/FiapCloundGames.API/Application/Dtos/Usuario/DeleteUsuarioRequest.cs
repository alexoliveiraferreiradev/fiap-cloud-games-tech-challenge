using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class DeleteUsuarioRequest
    {
        public Guid Id { get; set; }
        public MotivoExclusao MotivoDelecao { get; set; }

        public DeleteUsuarioRequest()
        {            
        }

        public DeleteUsuarioRequest(Guid id, MotivoExclusao motivoDelecao)
        {
            Id = id;
            MotivoDelecao = motivoDelecao;
        }
    }
        
}
