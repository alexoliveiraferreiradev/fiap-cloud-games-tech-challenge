using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.UnitTests.Fixtures;

namespace FiapCloundGames.UnitTests.Entities
{
    /// <summary>
    /// Conjunto de testes unitários para a entidade `Usuario`.
    /// Contém cenários de sucesso e validações de domínio para criação de usuários.
    /// </summary>
    public class UsuarioTests
    {
        private readonly UsuarioFixture _usuarioFixture;
        public UsuarioTests()
        {
            _usuarioFixture = new UsuarioFixture();
        }

        /// <summary>
        /// Testa a criação de um usuário com perfil de jogador válido.
        /// Verifica que a entidade é criada, que o perfil é `Jogador` e que os campos essenciais não estão vazios.
        /// </summary>
        [Fact(DisplayName = "Adicionar novo usuário - Perfil Jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_UsuarioJogadorValido_DeveCriarComSucesso()
        {
            //Arrange
            //Act             
            var usuario = _usuarioFixture.GerarUsuarioJogadorComSucesso();
            //Assert
            Assert.NotNull(usuario);
            Assert.Equal(TipoUsuario.Jogador, usuario.Perfil);
            Assert.False(string.IsNullOrEmpty(usuario.NomeUsuario));
            Assert.False(string.IsNullOrEmpty(usuario.Email));
        }

        /// <summary>
        /// Testa falha na criação quando o nome do usuário não é preenchido.
        /// Deve lançar <see cref="DomainException"/> com a mensagem esperada.
        /// </summary>
        [Fact(DisplayName = "Falha ao adicionar novo Usuário - Nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_NomeNaoPreenchido_DeveRetornarFalha()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.GerarUsuarioJogadorComNomeNaoPreenchido());
            //Assert
            Assert.Equal("O nome do usuário é obrigatório.", result.Message);
        }


        /// <summary>
        /// Testa falha na criação quando o email do usuário não é preenchido.
        /// Deve lançar <see cref="DomainException"/> com a mensagem esperada.
        /// </summary>
        [Fact(DisplayName = "Falha ao adicionar novo usuário - Email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_EmailJogadorNaoPreenchido_DeveRetornarFalha()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.GerarUsuarioJogadorComEmailNaoPreenchido());
            //Assert
            Assert.Equal("O email do usuário é obrigatório.", result.Message);
        }

     
        /// <summary>
        /// Testa falhas de validação de email inválido.
        /// Para cada email inválido passado, deve ser lançada uma <see cref="DomainException"/> com a mensagem apropriada.
        /// </summary>
        [Theory(DisplayName = "Falha ao adicionar novo Usuário - Email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void AdicionarUsuarioJogador_EmailInvalido_DeveRetornarFalha(string emailInvalido)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.GerarUsuarioJogadorEmailInvalido(emailInvalido));
            //Assert
            Assert.Equal("O email do usuário é inválido.", result.Message);
        }

        /// <summary>
        /// Testa falha quando a senha não atende aos requisitos de força.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de senha fraca.
        /// </summary>
        [Fact(DisplayName = "Falha ao adicionar novo Usuário - Senha fraca")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_SenhaFraca_DeveRetornarFalha()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.GerarUsuarioJogadorComSenhaInvalida());
            //Assert
            Assert.Equal("A senha deve conter pelo menos 8 caracteres, incluindo letras maiúsculas, minúsculas, números e caracteres especiais.", result.Message);
        }

        /// <summary>
        /// Testa falha quando a confirmação de senha é diferente da senha informada.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de confirmação divergente.
        /// </summary>
        [Fact(DisplayName = "Falha ao adicionar novo Usuário - Confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_ConfirmacaoDeSenhaDiferente_DeveRetornarFalha()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.GerarUsuarioJogadorComConfirmacaoDeSenhaDiferente());
            //Assert
            Assert.Equal("A senha e a confirmação de senha devem ser iguais.", result.Message);
        }
    }
}
