using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Usuario : AgreggateRoot
    {
        public Usuario()
        {

        }
        public string NomeUsuario { get; private set; }

        public string Email { get; private set; }

        public string Senha { get; private set; }

        public string reSenha { get; private set; }

        public TipoUsuario Perfil { get; set; }

        public Usuario(string nomeUsuario, string emailUsuario, string senhaUsuario,
            string confirmacaoSenhaUsuario)
        {
            NomeUsuario = nomeUsuario;
            Email = emailUsuario;
            Senha = senhaUsuario;
            reSenha = confirmacaoSenhaUsuario;
            Perfil = TipoUsuario.Jogador;
            Validar();
        }

        public void Validar()
        {
            AssertionConcern.AssertArgumentNotEmpty(NomeUsuario, "O nome do usuário é obrigatório.");
            AssertionConcern.AssertArgumentNotEmpty(Email, "O email do usuário é obrigatório.");
            AssertionConcern.AssertArgumentNotEmpty(Senha, "A senha do usuário é obrigatória.");
            AssertionConcern.AssertArgumentNotEmpty(reSenha, "A confirmação de senha do usuário é obrigatória.");
            AssertionConcern.AssertArgumentNotNull(Perfil, "O perfil do usuário é obrigatório.");
            AssertionConcern.AssertArgumentPasswordStrenght(Senha, "A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.");
            AssertionConcern.AssertArgumentPasswordStrenght(reSenha, "A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.");
            AssertionConcern.AssertArgumentEquals(Senha, reSenha, "A senha e a confirmação de senha devem ser iguais.");
            AssertionConcern.AssertArgumentEmailFormat(Email, "O email do usuário é inválido."); 
        }
    }
}
