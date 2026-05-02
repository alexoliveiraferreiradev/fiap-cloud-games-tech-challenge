using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public record CriaPromocaoRequest(
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        Guid jogoId,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        decimal valorPromocao,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        DateTime dataFim);
}
