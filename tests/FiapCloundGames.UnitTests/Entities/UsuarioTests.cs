using Bogus;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

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
        [Fact(DisplayName = "Sucesso ao adicionar novo usuário - perfil Jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_UsuarioJogadorValido_DeveCriarComSucesso()
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
        [Fact(DisplayName = "Falha ao adicionar novo Usuário - nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_NomeNaoPreenchido_DeveLancarExcecao()
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
        [Theory(DisplayName = "Falha ao adicionar novo Usuário - nome não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("aB")]
        [InlineData("xD")]
        public void AdicionarUsuarioJogador_NomeInvalido_DeveLancarExcecao(string nomeInvalido)
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
        [Fact(DisplayName = "Falha ao adicionar novo usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_EmailJogadorNaoPreenchido_DeveLancarExcecao()
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
        [Theory(DisplayName = "Falha ao adicionar novo usuário - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void AdicionarUsuarioJogador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioEmailInvalido(emailInvalido));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao adicionar novo usuário - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_SenhanaoPreenchida_DeveLancarExcecao()
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
        [Fact(DisplayName = "Falha ao adicionar novo usuário - senha fraca")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_SenhaFraca_DeveLancarExcecao()
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
        [Fact(DisplayName = "Falha ao adicionar novo Usuário - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_ConfirmacaoDeSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            //Act             
            var result = Assert.Throws<DomainException>(() => _usuarioFixture.ObtemUsuarioComConfirmacaoDeSenhaDiferente());
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }


        /// <summary>
        /// Testa a deleção de um usuário com perfil jogador.
        /// Verifica que o usuário é marcado como inativo quando dados válidos são fornecidos.
        /// </summary>
        [Fact(DisplayName = "Sucesso ao deletar o usuário - perfil jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void DeletarUsuario_DadosUsuarioValidos_DeveDeletarComSucesso()
        {
            //Arrange
            var usuario = new Usuario(_faker.Internet.Email(), "Teste@123");
            //Act             
            usuario.Deletar(usuario.Email, usuario.Senha);
            //Assert
            Assert.False(usuario.Ativo);
        }


        /// <summary>
        /// Verifica se a tentativa de deletar um usuário com um endereço de e-mail inválido resulta em uma exceção de
        /// domínio, indicando falha na operação conforme esperado.
        /// </summary>
        /// <remarks>Este teste garante que o método de exclusão de usuário lança uma exceção apropriada
        /// quando fornecido um e-mail em formato inválido, reforçando a validação de dados de entrada.</remarks>
        /// <param name="emailInvalido">O endereço de e-mail inválido a ser utilizado na tentativa de exclusão do usuário. Deve representar um
        /// formato de e-mail que não atende aos critérios de validação.</param>
        [Theory(DisplayName = "Falha ao adicionar novo Usuário - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("email_sem_arroba.com")]
        [InlineData("usuario@")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void DeletarUsuario_EmailUsuarioInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.Deletar(emailInvalido, usuario.Senha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailInvalido, result.Message);
        }

        /// <summary>
        /// Verifica se a tentativa de deletar um usuário jogador com o e-mail não preenchido retorna uma falha conforme
        /// esperado.
        /// </summary>
        /// <remarks>O teste assegura que a exceção DomainException é lançada quando o método Deletar é
        /// chamado com um e-mail inválido, validando o tratamento de entradas obrigatórias.</remarks>
        /// <param name="emailInvalido">O valor de e-mail inválido a ser testado. Deve representar um e-mail não preenchido ou inválido.</param>
        [Fact(DisplayName = "Falha ao deletar o usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void DeletarUsuario_EmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.Deletar(string.Empty, usuario.Senha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailObrigatorio, result.Message);
        }

        /// <summary>
        /// Verifica se o método Deletar lança uma exceção quando a senha não é fornecida.
        /// </summary>
        /// <remarks>Este teste garante que a tentativa de deletar um usuário sem informar a senha resulta
        /// em uma DomainException, validando a obrigatoriedade do preenchimento da senha para esta operação.</remarks>
        [Fact(DisplayName = "Falha ao deletar o usuário - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void DeletarUsuario_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.Deletar(usuario.Email, string.Empty));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }
        /// <summary>
        /// Verifica se o método Deletar lança uma exceção quando uma senha inválida é fornecida ao tentar deletar um
        /// usuário.
        /// </summary>
        /// <remarks>Este teste utiliza diferentes valores de senha inválida para garantir que o método
        /// Deletar do usuário lança uma DomainException quando a senha não é aceita. O teste cobre múltiplos cenários
        /// de senha fraca ou inadequada.</remarks>
        /// <param name="senhaInvalida">A senha considerada inválida para a operação de exclusão do usuário. Deve ser uma senha que não atende aos
        /// critérios de validação exigidos pelo domínio.</param>
        [Theory(DisplayName = "Falha ao deletar o usuário - senha inválida")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void DeletarUsuario_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.Deletar(usuario.Email, senhaInvalida));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }


        [Fact(DisplayName = "Sucesso ao atualizar email usuario - perfil jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarEmailJogador_UsuarioValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = _faker.Internet.Email();
            //Act
            usuario.AtualizarEmail(antigoEmail: usuario.Email, novoEmail: novoEmail);
            //Assert
            Assert.Equal(novoEmail, usuario.Email);
        }

        [Fact(DisplayName = "Falha ao atualizar o usuário - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarEmailJogador_NovoEmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = string.Empty;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarEmail(antigoEmail: usuario.Email, novoEmail: novoEmail));
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
        public void AtualizarEmailJogador_NovoEmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novoEmail = emailInvalido;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarEmail(antigoEmail: usuario.Email, novoEmail: novoEmail));
            //Assert    
            Assert.Equal(MensagensDominio.UsuarioEmailNovoInvalido, result.Message);
        }

        [Fact(DisplayName = "Sucesso ao atualizar senha do usuário - perfil jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaJogador_UsuarioValido_ComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = "NovaSenha@123";
            //Act
            usuario.AtualizarSenha(senhaAntiga: usuario.Senha, novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha);
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
        public void AtualizarSenhaJogador_NovaSenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = senhaInvalida;
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha(senhaAntiga: usuario.Senha, novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaFraca, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar senha do usuário - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaJogador_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = string.Empty;
            //Act 
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha(senhaAntiga: usuario.Senha, novaSenha: novaSenha, confirmacaoSenhaNova: novaSenha));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaObrigatoria, result.Message);
        }

        [Fact(DisplayName = "Falha ao atualizar senha do usuário - confirmação de nova senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AtualizarSenhaJogador_ConfirmacaoDeNovaSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var novaSenha = "NovaSenha@123";
            var confirmacaoNovaSenha = "Teste123";
            //Act
            var result = Assert.Throws<DomainException>(() => usuario.AtualizarSenha(senhaAntiga: usuario.Senha, novaSenha: novaSenha, confirmacaoSenhaNova: confirmacaoNovaSenha));
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

        
    }
}
