using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class DesativaUsuarioRequest
    {
        public Guid Id { get; set; }
        public MotivoExclusao MotivoDelecao { get; set; }

        public DesativaUsuarioRequest()
        {            
        }

        public DesativaUsuarioRequest(Guid id, MotivoExclusao motivoDelecao)
        {
            Id = id;
            MotivoDelecao = motivoDelecao;
        }
    }
        
}
