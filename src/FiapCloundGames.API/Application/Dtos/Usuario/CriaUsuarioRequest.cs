using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class CriaUsuarioRequest
    {
        /// <summary>
        /// Nome de acesso.
        /// </summary>
        /// <example>Nome do Usuário</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [RegularExpression(@"^(?i)(?!string$).*", ErrorMessage = DataAnnotationMessage.ErroNomeReal)]
        [StringLength(40, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 3)]
        public string Nome { get; set; }
        /// <summary>
        /// Endereço de e-mail do usuário cadastrado.
        /// </summary>
        /// <example>usuario@fiapgames.com.br</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [EmailAddress(ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [RegularExpression(@"^(?!.*@example\.com$).*", ErrorMessage = DataAnnotationMessage.ErroEmail)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string Email { get; set; }
        /// <summary>
        /// Senha de acesso.
        /// </summary>
        /// <example>SenhaSegura@123</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password, ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string Senha { get; set; }
        /// <summary>
        /// Confirmação de senha de acesso.
        /// </summary>
        /// <example>SenhaSegura@123</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password)]
        [Compare("Senha", ErrorMessage = DataAnnotationMessage.ErroConfirmacaoSenha)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
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