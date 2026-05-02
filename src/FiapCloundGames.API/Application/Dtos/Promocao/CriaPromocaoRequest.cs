using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public class CriaPromocaoRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public Guid JogoId { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal ValorPromocao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public DateTime DataFim { get; set; }

        public CriaPromocaoRequest()
        {
        }

        public CriaPromocaoRequest(Guid jogoid, decimal valorPromocao, DateTime dataFim)
        {
            JogoId = jogoid; ValorPromocao = valorPromocao; DataFim = dataFim;
        }
    }
}
