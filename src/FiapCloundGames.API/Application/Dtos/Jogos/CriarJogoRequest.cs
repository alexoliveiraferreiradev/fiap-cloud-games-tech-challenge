using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public class CriarJogoRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 2)]
        public string Nome { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(500, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string Descricao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal preco { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public GeneroJogo Genero { get; set; }
    }
}
