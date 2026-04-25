using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiapCloundGames.UnitTests.Entities
{
    public class UsuarioTests
    {
        [Fact(DisplayName = "Adicionar Novo Usuário")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuario_UsuarioValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = new Usuario("TesteUser", "teste@teste.com", "senha123", "senha123", PerfilUsuario.Administrador, "123456789");
            //Act 
            var result = usuario.Validar(usuario);
            //assert
            Assert.True(result);  
        }
    }
}
