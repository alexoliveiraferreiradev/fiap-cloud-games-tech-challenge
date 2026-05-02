using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Domain.Entities
{
    /// <summary>
    /// Conjunto de testes unitários para a entidade `Usuario`.
    /// Contém cenários de sucesso e validações de domínio para criação de usuários.
    /// </summary>
    public class UsuarioTests
    {
        private readonly Faker _faker;
        private readonly UsuarioFixture _usuarioFixture;
        public UsuarioTests()
        {
            _usuarioFixture = new UsuarioFixture();
            _faker = new Faker();
        }

        /// <summary>
        /// Testa a criação de um usuário com perfil de jogador válido.
        /// Verifica que a entidade é criada, que o perfil é `Jogador` e que os campos essenciais não estão vazios.
        /// </summary>
        [Fact(DisplayName = "Sucesso ao cadastrar novo usuário - perfil Jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_UsuarioJogadorValido_DeveCriarComSucesso()
        {
            //Arrange
            //Act             
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Assert
            Assert.NotNull(usuario);
            Assert.Equal(TipoUsuario.Jogador, usuario.Perfil);
            Assert.False(string.IsNullOrEmpty(usuario.NomeUsuario.Valor));
            Assert.False(string.IsNullOrEmpty(usuario.EmailUsuario.Valor));
        }

        [Fact(DisplayName = "Sucesso ao atualizar nome do usuário - nome válido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarNomeUsuario_UsuarioValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoNome = new Nome( _faker.Internet.UserName());
            //Act
            usuario.AtualizarNomeUsuario(nomeNovo: novoNome);
            //Assert
            Assert.Equal(novoNome.Valor, usuario.NomeUsuario.Valor);
        }            
        [Fact(DisplayName = "Sucesso ao alterar perfil do usuário - de jogador para administrador")]
        [Trait("Categoria", "Usuario Tests")]
        public void RebaixarPerfil_UsuarioEhAdmin_DeveRebaixarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemAdminComSucesso();
            //Act
            usuario.RebaixarPerfil();
            //Assert
            Assert.Equal(TipoUsuario.Jogador, usuario.Perfil);
        }

        [Fact(DisplayName = "Sucesso ao alterar perfil do usuário - de jogador para administrador")]
        [Trait("Categoria", "Usuario Tests")]
        public void RebaixarPerfil_UsuarioJaEhAdmin_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.RebaixarPerfil());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioPerfilRebaixarInvalido, result.Message);
        }

        [Fact(DisplayName = "Sucesso ao desativar usuário - deve desativar com sucesso")]
        [Trait("Categoria", "Usuario Tests")]
        public void DesativarUsuario_UsuarioAtivo_DeveDesativarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act
            usuario.Desativar(MotivoExclusao.Outros);
            //Assert
            Assert.False(usuario.Ativo);            
        }


        [Fact(DisplayName = "Falha ao desativar usuário - usuário é inativo")]
        [Trait("Categoria", "Usuario Tests")]
        public void DesativarUsuario_UsuarioInativo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemUsuarioInativo();
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.Desativar(MotivoExclusao.Outros));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioJaDesativado, result.Message);
        }


        [Fact(DisplayName = "Reativar usuário - reativar usuário com sucesso")]
        [Trait("Categoria", "Usuario Tests")]
        public void ReativarUsuario_UsuarioInativo_DeveReativarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemUsuarioInativo();
            //Act
            usuario.Reativar();
            //Assert
            Assert.True(usuario.Ativo);
        }


        [Fact(DisplayName = "Falha ao reativar usuário - usuário já está ativo")]
        [Trait("Categoria", "Usuario Tests")]
        public void ReativarUsuario_UsuarioAtivo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.Reativar());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioAtivo, result.Message);
        }


    }
}
