using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class UpdateUsuarioRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [RegularExpression(@"^(?i)(?!string$).*", ErrorMessage = DataAnnotationMessage.ErroNomeReal)]
        [StringLength(40, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 3)]
        public string NomeUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [EmailAddress(ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [RegularExpression(@"^(?!.*@example\.com$).*", ErrorMessage = DataAnnotationMessage.ErroEmail)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string EmailUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password, ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string SenhaUsuario { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password)]
        [Compare("SenhaUsuario", ErrorMessage = DataAnnotationMessage.ErroConfirmacaoSenha)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string ConfirmacaoSenha { get; set; }
        public UpdateUsuarioRequest()
        {             
        }
        public UpdateUsuarioRequest(string novoNome, string novoEmail, string novaSenha, string novaConfirmaoSenha)
        {
            NomeUsuario = novoNome; EmailUsuario = novoEmail; SenhaUsuario = novaSenha; ConfirmacaoSenha = novaConfirmaoSenha;
        }
    }
}
