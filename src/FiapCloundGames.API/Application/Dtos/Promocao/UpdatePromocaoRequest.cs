using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Promocao
{
    public record UpdatePromocaoRequest(
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        Guid jogoId,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        decimal novoValorPromocao,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        DateTime novaDataFim);
}
