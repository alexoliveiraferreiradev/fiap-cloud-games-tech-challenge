using Bogus;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Fixtures
{
    public class UsuarioFixture
    {
        private readonly Faker _faker;
        private Email _emailUsuario;
        public UsuarioFixture()
        {
            _faker = new Faker();
            _emailUsuario = new Email(_faker.Internet.Email());
        }

        public CriaUsuarioRequest UsuarioRequest()
        {
            return new CriaUsuarioRequest(
                Nome: _faker.Internet.UserName(),
                Email: _faker.Internet.Email(),
                Senha: "Teste@123",
                reSenha: "Teste@123"
            );
        }

        public CriaUsuarioRequest UsuarioRequestSenhaDiferente()
        {
            return new CriaUsuarioRequest(
                Nome: _faker.Internet.UserName(),
                Email: _faker.Internet.Email(),
                Senha: "Teste@123",
                reSenha: "Teste@1243"
            );
        }

        public Usuario ObtemJogadorComSucesso()
        {
            var nomeUsuario = _faker.Internet.UserName();
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }


        public Usuario ObtemUsuarioComNomeNaoPreenchido()
        {
            var nomeUsuario = string.Empty;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }
        public Usuario ObtemUsuarioComNomeInvalido(string nomeInvalido)
        {
            var nomeUsuario = nomeInvalido;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario ObtemUsuarioComSenhaInvalida()
        {
            var nomeUsuario = _faker.Internet.UserName();
            var senhaUsuario = "123";
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }
        public Usuario ObtemUsuarioComSenhaNaoPreenchida()
        {
            var nomeUsuario = _faker.Internet.UserName();
            var senhaUsuario = string.Empty;
            var confirmacaoSenhaUsuario = senhaUsuario;
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario ObtemUsuarioComConfirmacaoDeSenhaDiferente()
        {
            var nomeUsuario = _faker.Internet.UserName();
            var senhaUsuario = "Teste@123";
            var confirmacaoSenhaUsuario = senhaUsuario + "123";
            return new Usuario(nomeUsuario, _emailUsuario, senhaUsuario, confirmacaoSenhaUsuario);
        }

        public Usuario ObtemAdminComSucesso()
        {
            var usuario = new Usuario(_emailUsuario, "Teste@123");
            typeof(Usuario).GetProperty("Perfil").SetValue(usuario, TipoUsuario.Administrador);
            return usuario;
        }

        public Usuario ObtemUsuarioInativo()
        {
            
            var usuario = new Usuario(_emailUsuario, "Teste@123");
            typeof(Usuario).GetProperty("Ativo").SetValue(usuario, false);
            return usuario;
        }
    }
}
