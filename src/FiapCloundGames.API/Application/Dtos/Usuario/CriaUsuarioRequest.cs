using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class CriaUsuarioRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(40, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 3)]
        public string Nome { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [EmailAddress(ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string Email { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password, ErrorMessage = DataAnnotationMessage.ErroFormato)]
        public string Senha { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = DataAnnotationMessage.ErroConfirmacaoSenha)]
        public string ConfirmacaoSenha { get; set; }

        public CriaUsuarioRequest()
        {
        }

        public CriaUsuarioRequest(string nomeUsuario, string emailUsuario, string senhaUsuario, string reSenha)
        {
            Nome = nomeUsuario; Email = emailUsuario; Senha = senhaUsuario; ConfirmacaoSenha = reSenha;
        }
    }


}