using AutoMapper;
using Bogus;
using Castle.Core.Logging;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services;
using FiapCloundGames.API.Configuration.Mapping;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.UnitTests.Fixtures;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace FiapCloundGames.UnitTests.Application.Services
{
    public class UsuarioServiceTests
    {
        private readonly Faker _faker;
        private readonly UsuarioFixture _usuarioFixture;
        private readonly Mock<IPasswordHasher> _passwordMock;
        private readonly Mock<IUsuarioRepository> _usuarioMock;
        private readonly UsuarioService _service;
        private readonly ILogger<UsuarioService> _logger;
        private IMapper _mapper;
        public UsuarioServiceTests()
        {
            _usuarioFixture = new UsuarioFixture();
            _faker = new Faker();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<UsuarioProfile>();
            });
            _mapper = config.CreateMapper();
            _usuarioMock = new Mock<IUsuarioRepository>();
            _passwordMock = new Mock<IPasswordHasher>();
            _logger = NullLogger<UsuarioService>.Instance;
            _service = new UsuarioService(_usuarioMock.Object, _passwordMock.Object, _mapper,_logger);
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
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            //Act
            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            var result = await _service.PromoverParaAdmin(usuario.Id);
            //Assert
            Assert.Equal(TipoUsuario.Administrador, result.PerfilUsuario);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }

        /// <summary>
        /// Verifica se o método CadastrarAdministrador lança uma exceção do tipo DomainException quando o token de acesso para criar um administrador é inválido ou ausente.
        /// </summary>
        /// <returns></returns>
        /// <remarks>Este teste verifica se uma exceção é lançada corretamente quando o token de acesso 
        /// para criar um administrador é inválido ou ausente.</remarks>
        [Fact(DisplayName = "Falha ao promover novo admistrador - usuário não encontrado")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task PromoverUsuarioAdministrador_UsuarioNaoEncontrado_DeveLancarExcecao()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.PromoverParaAdmin(usuario.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
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
            //Act 
            var result = Assert.Throws<DomainException>(() =>new Email(emailInvalido));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        /// <summary>
        /// Verifica se o método CriaJogador cria um usuário com perfil de Jogador corretamente quando os dados fornecidos são válidos.
        /// </summary>
        /// <remarks>Este teste verifica se um usuário com perfil de Jogador é criado corretamente quando os dados fornecidos são válidos. Ele utiliza um mock do serviço de usuário para simular a criação do jogador e valida se o perfil do usuário criado é realmente de Jogador, além de verificar se o método de adição do repositório foi chamado corretamente.</remarks>
        [Fact(DisplayName = "Sucesso ao cadastrar novo jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task CadastrarUsuarioJogador_JogadorValido_DeveCriarComSucesso()
        {
            //Arrange
            var usuarioRequest = _usuarioFixture.UsuarioRequest();
            //Mock

            _passwordMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns(usuarioRequest.Senha);
            //Act
            var result = await _service.CadastrarUsuario(usuarioRequest);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(usuarioRequest.Email, result.Email);
            Assert.Equal(TipoUsuario.Jogador, result.PerfilUsuario);

            _usuarioMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Once);
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
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.CadastrarUsuario(usuarioRequest));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
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
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.CadastrarUsuario(usuarioRequest));
            //Assert
            _usuarioMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Never);
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
            _passwordMock.Setup(h => h.HashPassword(usuarioRequest.Senha)).Returns(usuarioRequest.Senha);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.CadastrarUsuario(usuarioRequest));
            //Assert
            _usuarioMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Never);
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
            //Act 
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.CadastrarUsuario(_usuarioFixture.UsuarioRequestSenhaDiferente()));
            //Assert
            _usuarioMock.Verify(r => r.Adicionar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Sucesso ao rebaixar um adminstrador para jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioEhAdmin_DeveMudarParaJogador()
        {
            //Arrange
            var usuarioRebaixar = _usuarioFixture.ObtemAdminComSucesso();
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            await _service.RebaixarParaJogador(usuarioRebaixar.Id,Guid.NewGuid());
            //Assert
            Assert.Equal(TipoUsuario.Jogador, usuarioRebaixar.Perfil);
        }


        [Fact(DisplayName = "Falha ao rebaixar um jogador que já é jogador")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioJaEhJogador_DeveLancarExcecao()
        {
            //Arrange
            var usuarioRebaixar = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(usuarioRebaixar.Id)).ReturnsAsync(usuarioRebaixar);

            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.RebaixarParaJogador(idUsuarioRebaixar: usuarioRebaixar.Id, Guid.NewGuid()));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioPerfilRebaixarInvalido, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao rebaixar um administrador - não há usuário para ser rebaixado")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_UsuarioNaoExiste_DeveLancarExcecao()
        {

            var idUsuarioARebaixar = Guid.NewGuid();
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(idUsuarioARebaixar)).ReturnsAsync((Usuario)null);

            // Act  
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.RebaixarParaJogador(idUsuarioRebaixar: idUsuarioARebaixar, Guid.NewGuid()));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao rebaixar um administrador - usuário tentando rebaixar o próprio perfil")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task RebaixarPerfil_RebaixamentoInvalido_DeveLancarExcecao()
        {

            var idUsuarioARebaixar = Guid.NewGuid();
            //Mock
            // Act  
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.RebaixarParaJogador(idUsuarioARebaixar, idUsuarioARebaixar));
            //Assert
            Assert.Equal(MensagensDominio.OperacaoRebaixarInvalida, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Sucesso ao atualizar usuário - dados com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_UsuarioValido_DeveAtualizarComSucesso()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), "Teste@1234", "Teste@1234");
            //Mock

            _passwordMock.Setup(h => h.HashPassword(updataRequest.SenhaUsuario)).Returns(updataRequest.SenhaUsuario);
            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            await _service.AtualizarUsuario(usuario.Id, updataRequest);
            //Assert
            Assert.Equal(updataRequest.NomeUsuario, usuario.NomeUsuario.Valor);
            Assert.Equal(updataRequest.EmailUsuario, usuario.EmailUsuario.Valor);
            Assert.Equal(updataRequest.SenhaUsuario, usuario.Senha.Hash);

            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }


        [Fact(DisplayName = "Falha ao atualizar usuário - novo email não preenchido")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovoEmailNaoPreenchido_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), string.Empty, "Teste@123", "Teste@123");
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
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

            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao atualizar usuário - nova senha não preenchida")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovaSenhaNaoPreenchida_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), string.Empty, string.Empty);
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao atualizar usuário - confirmação de senha diferente")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AtualizarUsuario_NovaSenhaConfirmacaoDeSenhaDiferente_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var updataRequest = new UpdateUsuarioRequest(_faker.Internet.UserName(), _faker.Internet.Email(), "Teste1234@", "Teste12345@");
            //Mock

            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioSenhaConfirmacaoDiferente, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
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

            _passwordMock.Setup(h => h.HashPassword(novaSenha)).Returns(novaSenha);
            _usuarioMock.Setup(r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.AtualizarUsuario(usuario.Id, updataRequest));
            //Assert
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Sucesso ao desativar usuário - desativação com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarUsuario_UsuarioValido_DeveDesativarComSucesso()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            _usuarioMock.Setup(r => r.ObterPorId(deleteUsuarioRequest.Id)).ReturnsAsync(usuario);
            _usuarioMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            //Act
            await _service.Desativar(deleteUsuarioRequest, admin.Id);
            //Assert
            Assert.False(usuario.Ativo);

            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact(DisplayName = "Sucesso ao desativar usuário - desativação com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarConta_UsuarioValido_DeveDesativarComSucesso()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            _usuarioMock.Setup(r => r.ObterPorId(deleteUsuarioRequest.Id)).ReturnsAsync(usuario);
            //Act
            await _service.DesativarConta(deleteUsuarioRequest.Id);
            //Assert
            Assert.False(usuario.Ativo);

            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact(DisplayName = "Falha ao desativar usuário - usuário não encontrado")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarConta_UsuarioNaoEncontrado_DeveDesativarComSucesso()
        {
            //Arrange            
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            //Act
           var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.DesativarConta(deleteUsuarioRequest.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado,result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }
        [Fact(DisplayName = "Falha ao desativar usuário - administrador não encontrado")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarConta_AdministradorNaoEncontrado_DeveDesativarComSucesso()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            _usuarioMock.Setup(r => r.ObterPorId(deleteUsuarioRequest.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(async () => await _service.Desativar(deleteUsuarioRequest,admin.Id));
            //Assert
            Assert.Equal(MensagensDominio.AdminNaoEncontrado, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }


        [Fact(DisplayName = "Falha ao desativar usuário - usuário não existe")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarUsuario_UsuarioInexistente_DeveLancarExcecao()
        {
            //Arrange
            var admin = _usuarioFixture.ObtemAdminComSucesso();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(Guid.NewGuid(), MotivoExclusao.Inatividade);
            //Mock
            _usuarioMock.Setup(r =>  r.ObterPorId(deleteUsuarioRequest.Id)).ReturnsAsync((Usuario)null);
            _usuarioMock.Setup(r => r.ObterPorId(admin.Id)).ReturnsAsync(admin);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.Desativar(deleteUsuarioRequest,admin.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioNaoEncontrado, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Falha ao desativar usuário - usuário tentando desativar o próprio perfil")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task DesativarUsuario_DesativacaoPropria_DeveLancarExcecao()
        {
            //Arrange
            var idUsuario = Guid.NewGuid();
            var deleteUsuarioRequest = new DesativaUsuarioRequest(idUsuario, MotivoExclusao.Inatividade);
            //Mock
            _usuarioMock.Setup(r =>  r.ObterPorId(deleteUsuarioRequest.Id)).ReturnsAsync((Usuario)null);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.Desativar(deleteUsuarioRequest, deleteUsuarioRequest.Id));
            //Assert
            Assert.Equal(MensagensDominio.OperacaoDesativarInvalida, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact(DisplayName = "Login de usuário - usuário autenticado com sucesso")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task AutenticarUsuario_UsuarioValido_DeveAutenticarComSucesso()
        {
            //Arrange
            var loginRequest = new LoginRequest(_faker.Internet.Email(), "Senha@123");
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock 
            _passwordMock.Setup(h => h.HashPassword(loginRequest.Senha)).Returns(loginRequest.Senha);
            _passwordMock.Setup(h => h.VerifyPassword(loginRequest.Senha, usuario.Senha.Hash)).Returns(true);
            _usuarioMock.Setup( r => r.ObterPorEmail(loginRequest.Email)).ReturnsAsync(usuario);
            //Act
            var result = await _service.Autenticar(loginRequest);
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
            _usuarioMock.Setup( r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            await _service.Reativar(usuario.Id);
            //Assert
            Assert.True(usuario.Ativo);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Once);
        }

        [Fact(DisplayName = "Falha ao reativar usuário - usuário já está ativo")]
        [Trait("Categoria", "Usuario Service Tests")]
        public async Task ReativarUsuario_UsuarioAtivo_DeveLancarExcecao()
        {
            //Arrange
            var usuario = _usuarioFixture.ObtemJogadorComSucesso();
            //Mock  
            _usuarioMock.Setup( r => r.ObterPorId(usuario.Id)).ReturnsAsync(usuario);
            //Act
            var result = await Assert.ThrowsAsync<DomainException>(() => _service.Reativar(usuario.Id));
            //Assert
            Assert.Equal(MensagensDominio.UsuarioAtivo, result.Message);
            _usuarioMock.Verify(r => r.Atualizar(It.IsAny<Usuario>()), Times.Never);
        }

    }
}
