using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class LoginRequest {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [EmailAddress(ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [RegularExpression(@"^(?!.*@example\.com$).*", ErrorMessage = DataAnnotationMessage.ErroEmail)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string Email { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string Senha { get; set; }
        public LoginRequest()
        {            
        }
        public LoginRequest(string emailUsuario, string senhaUsuario)
        {
            Email = emailUsuario;
            Senha = senhaUsuario;
        }
    }
}
