using FiapCloudGames.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Dtos.Usuario
{
    public class UpdateUsuarioRequest
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        /// <example>Novo nome do usuário</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [RegularExpression(@"^(?i)(?!string$).*", ErrorMessage = DataAnnotationMessage.ErroNomeReal)]
        [StringLength(40, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 3)]
        public string NomeUsuario { get; set; }      
        /// <summary>
        /// Senha do usuário
        /// </summary>
        /// <example>Nova senha do usuário</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [DataType(DataType.Password, ErrorMessage = DataAnnotationMessage.ErroFormato)]
        [StringLength(60, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 8)]
        public string SenhaUsuario { get; set; }
        /// <summary>
        /// Confirmação de senha
        /// </summary>
        /// <example>Confirmação de senha do usuário</example>
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
            NomeUsuario = novoNome; SenhaUsuario = novaSenha; ConfirmacaoSenha = novaConfirmaoSenha;
        }
    }
}
