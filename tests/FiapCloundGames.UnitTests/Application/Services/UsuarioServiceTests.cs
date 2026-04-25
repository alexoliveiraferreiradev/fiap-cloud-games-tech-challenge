using Bogus;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Infrastructure.Repository;
using FiapCloundGames.UnitTests.Fixtures;
using Moq;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class UsuarioServiceTests
    {
        private readonly Faker _faker;
        private readonly UsuarioFixture _usuarioFixture;
        public UsuarioServiceTests()
        {
            _usuarioFixture = new UsuarioFixture();
            _faker = new Faker();
        }

        /// <summary>
        /// Testes para criação de usuário com perfil de Administrador, validando as permissões e o token de acesso.
        /// </summary>
        /// <returns></returns>
        /// <remarks> Este teste verifica se um usuário com perfil de Administrador é criado corretamente quando as permissões e o
        /// token de acesso são válidos. Ele utiliza um mock do serviço de usuário para simular a criação do administrador e 
        /// valida se o perfil do usuário criado é realmente de Administrador.</remarks> 
        [Fact(DisplayName = "Adicionar novo admistrador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_AdministradorValido_DeveCriarComSucesso()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = service.CriaAdministrador(usuario, true, "INVITE-ADMIN-VALID");
            //Assert
            Assert.Equal(TipoUsuario.Administrador, result.Perfil);
        }

        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando as permissões para criar um administrador são inválidas.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando as permissões 
        /// para criar um administrador são inválidas.</remarks>
        [Fact(DisplayName = "Falha ao adicionar novo admistrador - não há permissões")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_AdministradorInvalidoSemPermissao_DeveLancarExcecao()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(usuario, false, "INVITE-ADMIN-VALID"));
            //Assert
            Assert.Equal(MensagensDominio.PermissaoNegadaCriarAdministrador, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando o token de acesso para criar um administrador é inválido ou ausente.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o token de acesso 
        /// para criar um administrador é inválido ou ausente.</remarks>
        [Fact(DisplayName = "Falha ao adicionar novo admistrador - não há token")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_AdministradorInvalidoSemToken_DeveLancarExcecao()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(usuario, true, ""));
            //Assert
            Assert.Equal(MensagensDominio.PermissaoNegadaCriarAdministrador, result.Message);
        }
        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando o email do usuário para criar um administrador não é preenchido.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um administrador não é preenchido.</remarks>
        [Fact(DisplayName = "Falha ao adicionar novo admistrador - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_EmailNãoPreenchido_DeveLancarExcecao()
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = string.Empty;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailObrigatorio, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando a senha do usuário para criar um administrador não é preenchida.
        /// </summary>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a senha do usuário para criar um administrador não é preenchida.</remarks>
        [Fact(DisplayName = "Falha ao adicionar novo admistrador - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = string.Empty;
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando a senha do usuário para criar um administrador é inválida.
        /// </summary>
        /// <param name="senhaInvalida"> A senha inválida a ser testada</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a senha do usuário para criar um administrador é inválida </remarks>
        [Theory(DisplayName = "Falha ao adicionar novo admistrador - senha inválida")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void AdicionarUsuarioAdministrador_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = senhaInvalida;
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }
        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando o email do usuário para criar um administrador é inválido.
        /// </summary>
        /// <param name="emailInvalido">O email inválido a ser testado.</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um administrador é inválido,
        /// utilizando diferentes formatos de email inválidos.</remarks>
        [Theory(DisplayName = "Falha ao adicionar novo admistrador - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("emailinvalido")]
        [InlineData("email@invalido")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void AdicionarUsuarioAdministrador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = emailInvalido;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailInvalido, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaAdministrador lança uma exceção do tipo DomainException quando a confirmação de senha do usuário para criar um administrador é diferente da senha.
        /// </summary>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a confirmação de senha do usuário para criar um administrador é diferente da senha </remarks>
        [Fact(DisplayName = "Falha ao adicionar novo admistrador - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioAdministrador_ConfirmacaoDeSenhaInvalida_DeveLancarExcecao()
        {
            //Arrange                       
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = Assert.Throws<DomainException>(() => service.CriaAdministrador(_usuarioFixture.ObtemUsuarioComConfirmacaoDeSenhaDiferente(), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaJogador cria um usuário com perfil de Jogador corretamente quando os dados fornecidos são válidos.
        /// </summary>
        /// <remarks>Este teste verifica se um usuário com perfil de Jogador é criado corretamente quando os dados fornecidos são válidos. Ele utiliza um mock do serviço de usuário para simular a criação do jogador e valida se o perfil do usuário criado é realmente de Jogador, além de verificar se o método de adição do repositório foi chamado corretamente.</remarks>
        [Fact(DisplayName = "Adicionar novo jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_JogadorValido_DeveCriarComSucesso()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = service.CriaJogador(usuario);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(usuario.Email, result.Email);
            Assert.Equal(TipoUsuario.Jogador, result.Perfil);

            repoMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Once);
        }
        /// <summary>
        /// Verifica se o método CriaJogador lança uma exceção do tipo DomainException quando o email do usuário para criar um jogador é inválido.
        /// </summary>
        /// <param name="emailInvalido">O email inválido a ser testado.</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um jogador é inválido, utilizando diferentes formatos de email inválidos.</remarks>
        [Theory(DisplayName = "Falha ao adicionar novo jogador - email inválido")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("emailinvalido")]
        [InlineData("email@invalido")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public void AdicionarUsuarioJogador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = emailInvalido;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = Assert.Throws<DomainException>(() => service.CriaJogador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha)));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailInvalido, result.Message);
        }

        /// <summary>
        /// Valida se o sistema impede a criação de um jogador quando o e-mail não é fornecido.
        /// </summary>
        /// <remarks>
        /// O teste garante que uma <see cref="DomainException"/> seja lançada ao tentar instanciar 
        /// um usuário com e-mail nulo ou vazio, preservando a integridade da entidade. </remarks>
        [Fact(DisplayName = "Falha ao adicionar novo jogador - email não preenchido")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_EmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = string.Empty;
            var senhaUsuario = "Teste@123";
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaJogador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha)));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailObrigatorio, result.Message);
        }
        /// <summary>
        /// Valida se a criação do jogador falha quando o campo de senha está em branco.
        /// </summary>
        /// <remarks>
        /// Verifica a obrigatoriedade da senha no construtor da entidade, disparando uma 
        /// exceção de domínio caso o argumento esteja ausente.
        /// </remarks>

        [Fact(DisplayName = "Falha ao adicionar novo jogador - senha não preenchida")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = string.Empty;
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaJogador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha)));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }
        /// <summary>
        /// Valida se o sistema rejeita senhas que não atendem aos critérios mínimos de segurança.
        /// </summary>
        /// <param name="senhaInvalida">A senha em formato incorreto fornecida pelo <see cref="InlineDataAttribute"/>.</param>
        /// <remarks>
        /// O teste percorre diversos cenários (falta de caracteres especiais, números, maiúsculas ou tamanho insuficiente) 
        /// para garantir que apenas senhas fortes sejam aceitas.
        /// </remarks>
        [Theory(DisplayName = "Falha ao adicionar novo jogador - senha inválida")]
        [Trait("Categoria", "Usuario Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public void AdicionarUsuarioJogador_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange            
            var nomeUsuario = _faker.Name.FullName();
            var emailUsuario = _faker.Internet.Email();
            var senhaUsuario = senhaInvalida;
            var confirmacaoSenha = senhaUsuario;
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act
            var result = Assert.Throws<DomainException>(() => service.CriaJogador(new Usuario(nomeUsuario, emailUsuario, senhaUsuario, confirmacaoSenha)));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }
        /// <summary>
        /// Valida se o sistema impede o cadastro quando a senha e a confirmação de senha são divergentes.
        /// </summary>
        /// <remarks>
        /// Garante a consistência dos dados de acesso, verificando se a lógica de comparação 
        /// no <see cref="AssertionConcern"/> bloqueia a criação do objeto com senhas distintas.
        /// </remarks>
        [Fact(DisplayName = "Falha ao adicionar novo jogador - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Tests")]
        public void AdicionarUsuarioJogador_ConfirmacaoDeSenhaInvalida_DeveLancarExcecao()
        {
            //Arrange                       
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() => service.CriaJogador(_usuarioFixture.ObtemUsuarioComConfirmacaoDeSenhaDiferente()));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }

        [Fact(DisplayName = "Sucesso ao rebaixar um adminstrador para jogador")]
        [Trait("Categoria","Usuario Tests")]
        public async Task RebaixarPerfil_UsuarioEhAdmin_DeveMudarParaJogador()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuarioRebaixar = _usuarioFixture.ObtemAdminComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            repoMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            await service.RebaixarPerfil(usuarioRebaixar.Id, admin.Id);
            //Assert
            Assert.Equal(TipoUsuario.Jogador, usuarioRebaixar.Perfil); 
        }


        [Fact(DisplayName = "Falha ao rebaixar um jogador que já é jogador")]
        [Trait("Categoria", "Usuario Tests")]
        public async Task RebaixarPerfil_UsuarioJaEhJogador_DeveLancarExcecao()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuarioRebaixar = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object);

            repoMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            repoMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.RebaixarPerfil(usuarioRebaixar.Id, admin.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioPerfilRebaixarInvalido, result.Message);
        }
    }
}
