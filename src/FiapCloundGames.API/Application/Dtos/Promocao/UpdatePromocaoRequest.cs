using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public class UpdatePromocaoRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public Guid JogoId { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal NovoValorPromocao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public DateTime NovaDataFim { get; set; }

        public UpdatePromocaoRequest()
        {
        }

        public UpdatePromocaoRequest(Guid jogoId, decimal valorPromocao, DateTime dataFim)
        {
            JogoId = jogoId; NovoValorPromocao = valorPromocao; NovaDataFim = dataFim;
        }
    }
}
