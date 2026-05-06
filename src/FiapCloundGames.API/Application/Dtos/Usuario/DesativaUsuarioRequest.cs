using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class DesativaUsuarioRequest
    {
        public Guid Id { get; set; }
        public MotivoDesativacao MotivoDelecao { get; set; }

        public DesativaUsuarioRequest()
        {            
        }

        public DesativaUsuarioRequest(Guid id, MotivoDesativacao motivoDelecao)
        {
            Id = id;
            MotivoDelecao = motivoDelecao;
        }
    }
        
}
