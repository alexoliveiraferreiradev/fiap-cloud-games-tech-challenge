using Bogus;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
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
        [Fact(DisplayName = "Promovendo novo admistrador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromovendoUsuarioAdministrador_AdministradorValido_DeveCriarComSucesso()
        {
            //Arrange            
            var usuarioRequest = _usuarioFixture.UsuarioRequest();
            //Mock
            var hashMock = new Mock<IPasswordHasher>();
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act
            var result = await service.CadastrarAdministrador(usuarioRequest, true, "INVITE-ADMIN-VALID");
            //Assert
            Assert.Equal(TipoUsuario.Administrador, result.Perfil);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando as permissões para criar um administrador são inválidas.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando as permissões 
        /// para criar um administrador são inválidas.</remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - não há permissões")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_AdministradorInvalidoSemPermissao_DeveLancarExcecao()
        {
            //Arrange            
            var usuario = _usuarioFixture.UsuarioRequest();
            //Mock
            var hashMock = new Mock<IPasswordHasher>();
            var repoMock = new Mock<IUsuarioRepository>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(usuario, false, "INVITE-ADMIN-VALID"));
            //Assert
            Assert.Equal(MensagensDominio.PermissaoNegadaCriarAdministrador, result.Message);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando o token de acesso para criar um administrador é inválido ou ausente.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o token de acesso 
        /// para criar um administrador é inválido ou ausente.</remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - não há token")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_AdministradorInvalidoSemToken_DeveLancarExcecao()
        {
            //Arrange            
            var usuario = _usuarioFixture.UsuarioRequest();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(usuario, true, ""));
            //Assert
            Assert.Equal(MensagensDominio.PermissaoNegadaCriarAdministrador, result.Message);
        }
        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando o email do usuário para criar um administrador não é preenchido.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um administrador não é preenchido.</remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - email não preenchido")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_EmailNãoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), string.Empty, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(usuarioRequest, true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailObrigatorio, result.Message);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando a senha do usuário para criar um administrador não é preenchida.
        /// </summary>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a senha do usuário para criar um administrador não é preenchida.</remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - senha não preenchida")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), _faker.Internet.Email(), string.Empty, string.Empty);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(usuarioRequest, true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaObrigatoria, result.Message);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando a senha do usuário para criar um administrador é inválida.
        /// </summary>
        /// <param name="senhaInvalida"> A senha inválida a ser testada</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a senha do usuário para criar um administrador é inválida </remarks>
        [Theory(DisplayName = "Falha ao promover novo admistrador - senha inválida")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public async Task PromoverUsuarioAdministrador_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), _faker.Internet.Email(), senhaInvalida, senhaInvalida);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(usuarioRequest, true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaFraca, result.Message);
        }
        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando o email do usuário para criar um administrador é inválido.
        /// </summary>
        /// <param name="emailInvalido">O email inválido a ser testado.</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um administrador é inválido,
        /// utilizando diferentes formatos de email inválidos.</remarks>
        [Theory(DisplayName = "Falha ao promover novo admistrador - email inválido")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("emailinvalido")]
        [InlineData("email@invalido")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public async Task PromoverUsuarioAdministrador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), emailInvalido, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = Assert.Throws<DomainException>(() =>new EmailUsuario(emailInvalido));
            //Assert
            Assert.Equal(MensagensDominio.EmailInvalido, result.Message);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando a confirmação de senha do usuário para criar um administrador é diferente da senha.
        /// </summary>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando a confirmação de senha do usuário para criar um administrador é diferente da senha </remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_ConfirmacaoDeSenhaInvalida_DeveLancarExcecao()
        {
            //Arrange                       
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarAdministrador(_usuarioFixture.UsuarioRequestSenhaDiferente(), true, ""));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }

        /// <summary>
        /// Verifica se o método CriaJogador cria um usuário com perfil de Jogador corretamente quando os dados fornecidos são válidos.
        /// </summary>
        /// <remarks>Este teste verifica se um usuário com perfil de Jogador é criado corretamente quando os dados fornecidos são válidos. Ele utiliza um mock do serviço de usuário para simular a criação do jogador e valida se o perfil do usuário criado é realmente de Jogador, além de verificar se o método de adição do repositório foi chamado corretamente.</remarks>
        [Fact(DisplayName = "Cadastrar novo jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuarioJogador_JogadorValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuarioRequest = _usuarioFixture.UsuarioRequest();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            hashMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns(usuarioRequest.Senha);
            //Act
            var result = await service.CadastrarJogador(usuarioRequest);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioRequest.Email, result.EmailUsuario.Email);
            Assert.Equal(TipoUsuario.Jogador, result.Perfil);

            repoMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Once);
        }
        /// <summary>
        /// Verifica se o método CriaJogador lança uma exceção do tipo DomainException quando o email do usuário para criar um jogador é inválido.
        /// </summary>
        /// <param name="emailInvalido">O email inválido a ser testado.</param>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o email do usuário para criar um jogador é inválido, utilizando diferentes formatos de email inválidos.</remarks>
        [Theory(DisplayName = "Falha ao cadastrar novo jogador - email inválido")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("emailinvalido")]
        [InlineData("email@invalido")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public async Task CadastrarUsuarioJogador_EmailInvalido_DeveLancarExcecao(string emailInvalido)
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), emailInvalido, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            hashMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns(usuarioRequest.Senha);
            //Act
            var result = Assert.Throws<DomainException>(() => new EmailUsuario(emailInvalido));
            //Assert
            Assert.Equal(MensagensDominio.EmailInvalido, result.Message);
        }

        /// <summary>
        /// Valida se o sistema impede a criação de um jogador quando o e-mail não é fornecido.
        /// </summary>
        /// <remarks>
        /// O teste garante que uma <see cref="DomainException"/> seja lançada ao tentar instanciar 
        /// um usuário com e-mail nulo ou vazio, preservando a integridade da entidade. </remarks>
        [Fact(DisplayName = "Falha ao cadastrar novo jogador - email não preenchido")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuarioJogador_EmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), string.Empty, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarJogador(usuarioRequest));
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

        [Fact(DisplayName = "Falha ao cadastrar novo jogador - senha não preenchida")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuarioJogador_SenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), _faker.Internet.Email(), string.Empty, string.Empty);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarJogador(usuarioRequest));
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
        [Theory(DisplayName = "Falha ao cadastrar novo jogador - senha inválida")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("senhaFraca")]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        [InlineData("@@@@@a")]
        [InlineData("senha@123")]
        [InlineData("SENHA@123")]
        public async Task CadastrarUsuarioJogador_SenhaInvalida_DeveLancarExcecao(string senhaInvalida)
        {
            //Arrange
            var usuarioRequest = new CriaUsuarioRequest(_faker.Name.FullName(), _faker.Internet.Email(), senhaInvalida, senhaInvalida);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            hashMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns(usuarioRequest.Senha);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarJogador(usuarioRequest));
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
        [Fact(DisplayName = "Falha ao cadastrar novo jogador - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuarioJogador_ConfirmacaoDeSenhaInvalida_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRequest = _usuarioFixture.UsuarioRequestSenhaDiferente();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await service.CadastrarJogador(_usuarioFixture.UsuarioRequestSenhaDiferente()));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
        }

        [Fact(DisplayName = "Cadastrar usuário  - deve criptografar a senha ao cadastrar")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuario_ValidacaoSenha_DeveCadastrarComSucesso()
        {
            //Arrange
            var usuarioRequest = _usuarioFixture.UsuarioRequest();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hasherMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hasherMock.Object);

            hasherMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns("HashSenha@123");

            //Act
            var result = await service.CadastrarJogador(usuarioRequest);
            //Arrange
            Assert.Equal("HashSenha@123", result.Senha);
            Assert.NotEqual("Teste@123", result.Senha);
        }

        [Fact(DisplayName = "Sucesso ao rebaixar um adminstrador para jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioEhAdmin_DeveMudarParaJogador()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuarioRebaixar = _usuarioFixture.ObtemAdminComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            repoMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            await service.RebaixarPerfil(usuarioRebaixar.Id, admin.Id);
            //Assert
            Assert.Equal(TipoUsuario.Jogador, usuarioRebaixar.Perfil);
        }


        [Fact(DisplayName = "Falha ao rebaixar um jogador que já é jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioJaEhJogador_DeveLancarExcecao()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuarioRebaixar = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            repoMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.RebaixarPerfil(idUsuarioRebaixar: usuarioRebaixar.Id, idAdminExecutor: admin.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioPerfilRebaixarInvalido, result.Message);
        }

        [Fact(DisplayName = "Falha ao rebaixar um administrador - não há administrador para rebaixar")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_AdministradorNaoExiste_DeveLancarExcecao()
        {
            var idAdminInexistente = Guid.NewGuid();
            var usuarioRebaixar = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);
            repoMock.Setup(r => r.ObterPorId(idAdminInexistente))
            .ReturnsAsync((Usuario)null);
            // Act & Assert
            var ex = await Assert.ThrowsAsync<DomainException>(() => service.RebaixarPerfil(idUsuarioRebaixar: usuarioRebaixar.Id, idAdminExecutor: idAdminInexistente));

            Assert.Equal(MensagensDominio.PermissaoNegadaCriarAdministrador, ex.Message);
        }


        [Fact(DisplayName = "Falha ao rebaixar um administrador - não há administrador para ser rebaixado")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioNaoExiste_DeveLancarExcecao()
        {
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var idUsuarioARebaixar = Guid.NewGuid();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            repoMock.Setup(r => r.ObterPorId(idUsuarioARebaixar)).ReturnsAsync((Usuario)null);

            // Act  
            var result = await Assert.ThrowsAsync<DomainException>(() => service.RebaixarPerfil(idUsuarioRebaixar: idUsuarioARebaixar, idAdminExecutor: admin.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
        }

        [Fact(DisplayName = "Atualizar usuário - dados com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_UsuarioValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), "Teste@1234", "Teste@1234");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            hashMock.Setup(h => h.HashPassword(updataRequest.senhaUsuario)).Returns(updataRequest.senhaUsuario);
            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            await service.AtualizarUsuario(usuario.Id, updataRequest);
            //Assert
            Assert.Equal(updataRequest.nomeUsuario, usuario.NomeUsuario);
            Assert.Equal(updataRequest.emailUsuario, usuario.EmailUsuario.Email);
            Assert.Equal(updataRequest.senhaUsuario, usuario.Senha);

            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao atualizar usuário - novo email não preenchido")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovoEmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), string.Empty, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailNovoObrigatorio, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Theory(DisplayName = "Falha ao atualizar usuário - novo email inválido")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("email@invalido")]
        [InlineData("@dominio.com")]
        [InlineData("usuario@dominio")]
        [InlineData("usuario@dominio..com")]
        public async Task AtualizarUsuario_NovoEmailInvalido_DeveLancarExcecao(string novoEmail)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), novoEmail, "Teste@123", "Teste@123");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioEmailNovoInvalido, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao atualizar usuário - nova senha não preenchida")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovaSenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), string.Empty, string.Empty);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaObrigatoria, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao atualizar usuário - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovaSenhaConfirmacaoDeSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), "Teste1234@", "Teste12345@");
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Theory(DisplayName = "Falha ao atualizar usuário - nova senha inválida")]
        [Trait("Categoria", "Usuario Service Tests")]
        [InlineData("senha")]
        [InlineData("123456")]
        [InlineData("abcdef")]
        [InlineData("senha123")]
        [InlineData("SENHA123")]
        public async Task AtualizarUsuario_NovaSenhaInvalida_DeveLancarExcecao(string novaSenha)
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), novaSenha, novaSenha);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);

            hashMock.Setup(h => h.HashPassword(novaSenha)).Returns(novaSenha);
            repoMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaNovaFraca, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Desativar usuário - desativação com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarUsuario_UsuarioValido_DeveDesativarComSucesso()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var deleteUsuarioRequest = new DeleteUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            repoMock.Setup(r => r.ObterPorId(deleteUsuarioRequest.id)).ReturnsAsync(usuario);
            //Act
            await service.Desativar(deleteUsuarioRequest);
            //Assert
            Assert.False(usuario.Ativo);

            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }


        [Fact(DisplayName = "Desativar usuário - usuário não existe")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarUsuario_UsuarioInexistente_DeveLancarExcecao()
        {
            //Arrange
            var idUsuario = Guid.NewGuid();
            var deleteUsuarioRequest = new DeleteUsuarioRequest(idUsuario, MotivoExclusao.Inatividade);
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hashMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object, hashMock.Object);
            repoMock.Setup(r =>  r.ObterPorId(deleteUsuarioRequest.id)).ReturnsAsync((Usuario)null);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.Desativar(deleteUsuarioRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            repoMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Login de usuário - usuário autenticado com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AutenticarUsuario_UsuarioValido_DeveAutenticarComSucesso()
        {
            //Arrange
            var loginRequest = new LoginRequest(_faker.Internet.Email(), "Senha@123");
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hasherMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object,hasherMock.Object);    
            hasherMock.Setup(h => h.HashPassword(loginRequest.senhaUsuario)).Returns(loginRequest.senhaUsuario);
            hasherMock.Setup(h => h.VerifyPassword(loginRequest.senhaUsuario, usuario.Senha)).Returns(true);
            repoMock.Setup( r => r.ObterPorEmail(loginRequest.emailUsuario)).ReturnsAsync(usuario);
            //Act
            var result = await service.Autenticar(loginRequest);
            //Assert
            Assert.NotNull(result);
        }


        [Fact(DisplayName = "Reativar usuário - usuário reativado com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task ReativarUsuario_UsuarioInativo_DeveReativarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemUsuarioInativo();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hasherMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object,hasherMock.Object);    
            repoMock.Setup( r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            await service.Reativar(usuario.Id);
            //Assert
            Assert.True(usuario.Ativo);
        }

        [Fact(DisplayName = "Falha ao reativar usuário - usuário já está ativo")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task ReativarUsuario_UsuarioAtivo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            var repoMock = new Mock<IUsuarioRepository>();
            var hasherMock = new Mock<IPasswordHasher>();
            var service = new UsuarioService(repoMock.Object,hasherMock.Object);    
            repoMock.Setup( r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => service.Reativar(usuario.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioAtivo, result.Message);
        }

    }
}
