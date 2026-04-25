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

        public PerfilUsuario Perfil { get; set; }

        public string Documento { get; private set; }
        public Usuario(string nomeUsuario, string emailUsuario, string senhaUsuario, 
            string confirmacaoSenhaUsuario, PerfilUsuario perfilUsuario, string documentoUsuario)
        {
            NomeUsuario = nomeUsuario;
            Email = emailUsuario;   
            Senha = senhaUsuario;
            reSenha = confirmacaoSenhaUsuario;
            Documento = documentoUsuario;
            Perfil = perfilUsuario; 
            Validar(this);
        }

        public bool Validar(Usuario usuario)
        {
            return true;
        }
    }
}
