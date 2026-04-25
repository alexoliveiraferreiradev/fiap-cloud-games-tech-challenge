using Bogus;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.UnitTests.Fixtures
{
    public class UsuarioFixture
    {
        private readonly Faker _faker;
        public UsuarioFixture()
        {
            _faker = new Faker();
        }

        public Usuario GerarUsuarioJogadorComSucesso()
        {
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario GerarUsuarioJogadorEmailInvalido(string emailInvalido)
        {
            var usuario = new Usuario();
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = emailInvalido;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario GerarUsuarioJogadorComEmailNaoPreenchido()
        {
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = string.Empty;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

       

        public Usuario GerarUsuarioJogadorComNomeNaoPreenchido()
        {
            var nomeUsuario = string.Empty;
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario GerarUsuarioJogadorComSenhaInvalida()
        {
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = "123"; 
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }
        public Usuario GerarUsuarioJogadorComConfirmacaoDeSenhaDiferente()
        {
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario + "123"; 
            return new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }
    }
}
