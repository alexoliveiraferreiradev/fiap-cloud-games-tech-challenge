using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record CriarJogoRequest(
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(100,ErrorMessage = DataAnnotationMessage.ErroCaracteres,MinimumLength =2)]
        string Nome,
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(500,ErrorMessage = DataAnnotationMessage.ErroCaracteres,MinimumLength =7)]
        string Descricao,
        [Required(ErrorMessage =DataAnnotationMessage.ErroRequired)]
        decimal preco,
        [Required(ErrorMessage =DataAnnotationMessage.ErroRequired)]
        GeneroJogo Genero);
}
