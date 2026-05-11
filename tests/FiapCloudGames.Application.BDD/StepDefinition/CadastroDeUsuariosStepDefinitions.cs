using AutoMapper;
using Bogus;
using FiapCloudGames.Application.Dtos.Identity;
using FiapCloudGames.Application.Dtos.Usuario;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Common.Exceptions;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Domain.Resources;
using FiapCloudGames.Infrastructure.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Reqnroll;
using Reqnroll.Assist;

namespace FiapCloudGames.Application.BDD.StepDefinition
{
    [Binding]
    public class CadastroDeUsuariosStepDefinitions
    {
        private Faker _faker = new Faker();
        private CriaUsuarioRequest _usuario;
        private string _mensagemDeErroRetornada;
        private readonly Mock<IPasswordHasherService> _passwordMock = new Mock<IPasswordHasherService>();
        private readonly Mock<IUsuarioRepository> _usuarioMock = new Mock<IUsuarioRepository>();
        private readonly Mock<ITokenService> _tokenService = new Mock<ITokenService>();
        private JwtTokenService _tokenGenerateService;
        private ILogger<UsuarioService> _logger;
        private ILogger<JwtTokenService> _loggerToken;
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();
        private UsuarioService _cadastroService;
        private UsuarioResponse _usuarioResponse;
        private LoginResponse _loginResponse;
        [Given("que o usuário está na página de cadastro")]
        public void GivenQueOUsuarioEstaNaPaginaDeCadastro()
        {           
            
            _usuario = new CriaUsuarioRequest();
            _usuarioResponse = new UsuarioResponse();
            _loginResponse = new LoginResponse();
            _logger = NullLogger<UsuarioService>.Instance;
       
            _cadastroService = new UsuarioService(_usuarioMock.Object, _passwordMock.Object, _mapperMock.Object, _logger, _tokenService.Object);
        }

        [When("o usuário submete o formulário com dados válidos")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidos()
        {
            _usuario.Nome = "AlexOliveira";
            _usuario.Email = "alex@fiap.com.br";
            _usuario.ConfirmacaoSenha = _usuario.Senha = "Senha@123";
            _usuarioResponse = new UsuarioResponse { Email = _usuario.Email, Nome = _usuario.Nome };
          
            _passwordMock.Setup(h => h.HashPassword(_usuario.Senha)).Returns(_usuario.Senha);
            _mapperMock.Setup(m => m.Map<UsuarioResponse>(It.IsAny<Usuario>()))
            .Returns(_usuarioResponse);
            var tokenResult = new TokenResult { AccessToken = "token-gerado-pelo-test" };
            _tokenService.Setup(s => s.GerarToken(It.IsAny<UsuarioResponse>()))
                    .ReturnsAsync(tokenResult);

            _loginResponse = await _cadastroService.Cadastrar(_usuario); 
        }

        [Then("o sistema deve registrar o usuário com sucesso")]
        public void ThenOSistemaDeveRegistrarOUsuarioComSucesso()
        {
            Assert.NotEmpty(_loginResponse.AcessToken);
            Assert.Equal("alex@fiap.com.br", _loginResponse.Email);
        }

        [Then("deve gerar um token de acesso válido")]
        public void ThenDeveGerarUmTokenDeAcessoValido()
        {
            Assert.NotNull(_loginResponse);

            Assert.False(string.IsNullOrEmpty(_loginResponse.AcessToken), "O Token de acesso não deveria estar vazio.");

            Assert.Equal("token-gerado-pelo-test", _loginResponse.AcessToken);
        }

        [When("o usuário submete o formulário com dados válidos de email e senha, porém não preenche o nome")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeEmailESenhaPoremNaoPreencheONome()
        {
            try
            {
                _usuario.Nome = ""; 
              await  _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("o sistema não registra o usuário")]
        public void ThenOSistemaNaoRegistraOUsuario()
        {
            _usuarioMock.Verify(repo => repo.Adicionar(It.IsAny<Usuario>()), Times.Never);
        }

        [Then("retorna a mensagem de erro sobre o nome obrigatório ao usuário")]
        public void ThenRetornaAMensagemDeErroSobreONomeObrigatorioAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.UsuarioNomeObrigatorio;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de email e senha, porém preenche um nome com tamanho de caracteres maior ou menor que o permitido")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeEmailESenhaPoremPreencheUmNomeComTamanhoDeCaracteresMaiorOuMenorQueOPermitido()
        {
            try
            {
                _usuario.Nome = "a";
                _usuario.Email = _faker.Internet.Email();
                _usuario.Senha = _faker.Internet.Password();    
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna a mensagem de erro sobre o nome tamanho inválido ao usuário")]
        public void ThenRetornaAMensagemDeErroSobreONomeTamanhoInvalidoAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.UsuarioTamanhoNomeInvalido;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e senha, porém não preenche o email")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioESenhaPoremNaoPreencheOEmail()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = string.Empty;
                _usuario.Senha = _faker.Internet.Password();
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna mensagem de erro sobre o email obrigatório ao usuário")]
        public void ThenRetornaMensagemDeErroSobreOEmailObrigatorioAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.EmailObrigatorio;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e senha, porém com o tamanho de caractere maior ou menor que o permitido")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioESenhaPoremComOTamanhoDeCaractereMaiorOuMenorQueOPermitido()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = _faker.Random.String(101);
                _usuario.Senha = _faker.Internet.Password();
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna mensagem de erro sobre o email de tamanho inválido ao usuário")]
        public void ThenRetornaMensagemDeErroSobreOEmailDeTamanhoInvalidoAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.EmailTamanhoInvalido;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e senha, porém o formato do email preenchido está incorreto")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioESenhaPoremOFormatoDoEmailPreenchidoEstaIncorreto()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = "teste@email..com";
                _usuario.Senha = _faker.Internet.Password();
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna mensagem de erro sobre o email em formato inválido ao usuário")]
        public void ThenRetornaMensagemDeErroSobreOEmailEmFormatoInvalidoAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.EmailInvalido;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e email, porém a senha não é preenchida")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioEEmailPoremASenhaNaoEPreenchida()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = _faker.Internet.Email();
                _usuario.Senha = string.Empty;
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna a mensagem de erro sobre a senha não preenchida ao usuário")]
        public void ThenRetornaAMensagemDeErroSobreASenhaNaoPreenchidaAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.UsuarioSenhaObrigatoria;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e email, porém o tamanho de caracteres da senha está inválido")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioEEmailPoremOTamanhoDeCaracteresDaSenhaEstaInvalido()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = _faker.Internet.Email();
                _usuario.Senha = "@12";
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna a mensagem de erro sobre tamanho de senha inválido ao usuário")]
        public void ThenRetornaAMensagemDeErroSobreTamanhoDeSenhaInvalidoAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.SenhaTamanhoInvalido;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete o formulário com dados válidos de nome de usuário e email, porém preenche uma senha fraca")]
        public async Task WhenOUsuarioSubmeteOFormularioComDadosValidosDeNomeDeUsuarioEEmailPoremPreencheUmaSenhaFraca()
        {
            try
            {
                _usuario.Nome = _faker.Internet.UserName();
                _usuario.Email = _faker.Internet.Email();
                _usuario.Senha = "senha123@";
                await _cadastroService.Cadastrar(_usuario);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna a mensagem de erro sobre senha informada é fraca ao usuário")]
        public void ThenRetornaAMensagemDeErroSobreSenhaInformadaEFracaAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.UsuarioSenhaFraca;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }
    }
}
