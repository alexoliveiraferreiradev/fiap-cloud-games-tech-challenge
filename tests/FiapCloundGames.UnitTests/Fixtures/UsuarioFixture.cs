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
        private Nome _nomeUsuario;
        private Senha _senhaUsuario;
        public UsuarioFixture()
        {
            _faker = new Faker();
            _emailUsuario = new Email(_faker.Internet.Email());
            _nomeUsuario = new Nome(_faker.Internet.UserName());
            _senhaUsuario = new Senha("Teste@123");
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
            return new Usuario(_nomeUsuario, _emailUsuario, _senhaUsuario);
        }

        public Usuario ObtemAdminComSucesso()
        {
            var usuario = new Usuario(_emailUsuario, _senhaUsuario);
            typeof(Usuario).GetProperty("Perfil").SetValue(usuario, TipoUsuario.Administrador);
            return usuario;
        }

        public Usuario ObtemUsuarioInativo()
        {            
            var usuario = new Usuario(_emailUsuario, _senhaUsuario);
            typeof(Usuario).GetProperty("Ativo").SetValue(usuario, false);
            return usuario;
        }
    }
}
