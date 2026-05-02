using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public class CriaPromocaoRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public Guid jogoId { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal valorPromocao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public DateTime dataFim { get; set; }
    }
}
