using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record UpdateJogosRequest(
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(100,ErrorMessage = DataAnnotationMessage.ErroCaracteres,MinimumLength =2)]
        string novoNome,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(500,ErrorMessage = DataAnnotationMessage.ErroCaracteres,MinimumLength =7)]
        string novaDescricao,
        [Required(ErrorMessage =DataAnnotationMessage.ErroRequired)]
        decimal novoPreco,
        [Required(ErrorMessage =DataAnnotationMessage.ErroRequired)]
        GeneroJogo novoGenero);
}
