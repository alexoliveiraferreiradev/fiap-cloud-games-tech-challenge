using Bogus;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.UnitTests.Entities
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
            Assert.False(string.IsNullOrEmpty(usuario.NomeUsuario));
            Assert.False(string.IsNullOrEmpty(usuario.Email));
        }


        /// <summary>
        /// Testa falha na criação quando o nome do usuário não é preenchido.
        /// Deve lançar <see cref="DomainException"/> com a mensagem esperada.
        /// </summary>
        [Fact(DisplayName = "Falha ao cadastrar novo Usuário - nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_NomeNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComNomeNaoPreenchido());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNomeObrigatorio, result.Message);
        }
        /// <summary>
        /// Testa falha na criação quando nome de usuário é inválido
        /// </summary>
        /// <param name="nomeInvalido"></param>
        [Theory(DisplayName = "Falha ao cadastrar novo Usuário - nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("aB")]
        [InlineData("xD")]
        public void CadastrarUsuarioJogador_NomeInvalido_DeveLancarExcecao(string nomeInvalido)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComNomeInvalido(nomeInvalido));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioTamanhoNomeInvalido, result.Message);
        }


        /// <summary>
        /// Testa falha na criação quando o email do usuário não é preenchido.
        /// Deve lançar <see cref="DomainException"/> com a mensagem esperada.
        /// </summary>
        [Fact(DisplayName = "Falha ao cadastrar novo usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_EmailJogadorNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComEmailNaoPreenchido());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailObrigatorio, result.Message);
        }


        /// <summary>
        /// Testa falhas de validação de email inválido.
        /// Para cada email inválido passado, deve ser lançada uma <see cref="DomainException"/> com a mensagem apropriada.
        /// </summary>
        [Theory(DisplayName = "Falha ao cadastrar novo usuário - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void CadastrarUsuarioJogador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioEmailInvalido(emailInvalido));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao cadastrar novo usuário - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_SenhanaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComSenhaNaoPreenchida());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }

        /// <summary>
        /// Testa falha quando a senha não atende aos requisitos de força.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de senha fraca.
        /// </summary>
        [Fact(DisplayName = "Falha ao cadastrar novo usuário - senha fraca")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_SenhaFraca_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComSenhaInvalida());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }

        /// <summary>
        /// Testa falha quando a confirmação de senha é diferente da senha informada.
        /// Deve lançar <see cref="DomainException"/> com a mensagem de confirmação divergente.
        /// </summary>
        [Fact(DisplayName = "Falha ao cadastrar novo Usuário - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void CadastrarUsuarioJogador_ConfirmacaoDeSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComConfirmacaoDeSenhaDiferente());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }


        [Fact(DisplayName = "Sucesso ao atualizar nome do usuário - nome válido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarNomeUsuario_UsuarioValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoNome = _faker.Internet.UserName();
            //Act
            usuario.AtualizarNomeUsuario(nomeNovo: novoNome);
            //Assert
            Assert.Equal(novoNome, usuario.NomeUsuario);
        }

        [Fact(DisplayName = "Falha ao atualizar nome do usuário - nome novo não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarNomeUsuario_NomeNovoNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoNome = string.Empty;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarNomeUsuario(nomeNovo: novoNome));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNomeNovoObrigatorio, result.Message);
        }

       

        [Fact(DisplayName = "Falha ao atualizar nome do usuário - nome novo inválido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarNomeUsuario_NomeNovoInvalido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoNome = _faker.Random.String(21);
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarNomeUsuario(nomeNovo: novoNome));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioTamanhoNomeInvalido, result.Message);
        }


        [Fact(DisplayName = "Sucesso ao atualizar email do usuário - email válido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarEmailUsuario_UsuarioValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = _faker.Internet.Email();
            //Act
            usuario.AtualizarEmail(novoEmail: novoEmail);
            //Assert
            Assert.Equal(novoEmail, usuario.Email);
        }

        [Fact(DisplayName = "Falha ao atualizar o usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarEmailUsuario_NovoEmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = string.Empty;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarEmail( novoEmail: novoEmail));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailNovoObrigatorio, result.Message);
        }

        [Theory(DisplayName = "Falha ao atualizar o usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void AtualizarEmailUsuario_NovoEmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = emailInvalido;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarEmail(novoEmail: novoEmail));
            //Assert    
            Assert.Equal(MensagensDominio.UsuarioEmailNovoInvalido, result.Message);
        }

        [Fact(DisplayName = "Sucesso ao atualizar senha do usuário - senha válida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaUsuario_UsuarioValido_ComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = "NovaSenha@123";
            //Act
            usuario.AtualizarSenha(novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha);
            //Assert
            Assert.Equal(novaSenha, usuario.Senha);

        }

        [Theory(DisplayName = "Falha ao atualizar senha do usuário - senha inválida")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void AtualizarSenhaUsuario_NovaSenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = senhaInvalida;
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha( novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaFraca, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar senha do usuário - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaUsuario_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = string.Empty;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha(novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaObrigatoria, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar senha do usuário - confirmação de nova senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaUsuario_ConfirmacaoDeNovaSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = "NovaSenha@123";
            var confirmacaoNovaSenha = "Teste123";
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha(novaSenha: novaSenha, confirmacaoSenhaNova: confirmacaoNovaSenha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);

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

        [Fact(DisplayName = "Sucesso ao reativar usuário - deve reativar com sucesso")]
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
        public void ReativarUsuario_UsuarioJaAtivo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.Reativar());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioJaAtivo, result.Message);
        }



    }
}
