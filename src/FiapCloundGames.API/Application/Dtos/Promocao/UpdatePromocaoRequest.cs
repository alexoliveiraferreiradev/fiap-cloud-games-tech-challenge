using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public class UpdatePromocaoRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public Guid jogoId { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal novoValorPromocao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public DateTime novaDataFim { get; set; }
    }
}
