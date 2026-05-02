using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class UpdateUsuarioRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(40, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 3)]
        public string NomeUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [EmailAddress(ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string EmailUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password, ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string SenhaUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = DataAnnotationMessage.ErroConfirmacaoSenha)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string ConfirmacaoSenha { get; set; }
    }
}
