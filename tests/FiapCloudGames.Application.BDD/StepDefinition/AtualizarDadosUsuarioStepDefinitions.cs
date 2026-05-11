using AutoMapper;
using Bogus;
using FiapCloudGames.Application.Dtos.Usuario;
using FiapCloudGames.Application.Interfaces;
using FiapCloudGames.Application.Mappings;
using FiapCloudGames.Application.Services;
using FiapCloudGames.Domain.Common.Exceptions;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Domain.Resources;
using FiapCloudGames.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Reqnroll;
using Reqnroll.Assist;
using System;
using System.Threading.Tasks;

namespace FiapCloudGames.Application.BDD.StepDefinition
{
    [Binding]
    public class AtualizarDadosUsuarioStepDefinitions
    {
        private Faker _faker;
        private readonly Mock<IUsuarioRepository> _usuarioMock = new Mock<IUsuarioRepository>();
        private UpdateUsuarioRequest _updateUsuarioRequest;
        private UsuarioService _atualizaService;
        private readonly Mock<IPasswordHasherService> _passwordMock = new Mock<IPasswordHasherService>();
        private ILogger<UsuarioService> _logger;
        private readonly Mock<ITokenService> _tokenService = new Mock<ITokenService>();
        private IMapper _mapper;
        private UsuarioResponse _usuarioResponse;
        private Usuario _usuario;
        private string _mensagemDeErroRetornada;
        [Given("que o usuário já está cadastrado e quer atualizar suas informações")]
        public void GivenQueOUsuarioJaEstaCadastradoEQuerAtualizarSuasInformacoes()
        {
            _faker = new Faker();
            _logger = NullLogger<UsuarioService>.Instance;
            _usuarioResponse = new UsuarioResponse();
            _updateUsuarioRequest = new UpdateUsuarioRequest();
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<UsuarioProfile>();
            });
            _mapper = config.CreateMapper();
            _atualizaService = new UsuarioService(_usuarioMock.Object, _passwordMock.Object, _mapper, _logger, _tokenService.Object);
            _mensagemDeErroRetornada = string.Empty;
        }

        [When("o usuário submete um novo nome de usuário válido")]
        public async Task WhenOUsuarioSubmeteUmNovoNomeDeUsuarioValido()
        {
            _updateUsuarioRequest = new UpdateUsuarioRequest("Novo nome usuario", _faker.Internet.Email(), "NovaSenha@123", "NovaSenha@123");
            _usuario = new Usuario(new Email("teste@fiap.com"), new Senha("Senha@123"));
            _passwordMock.Setup(h => h.HashPassword(_updateUsuarioRequest.SenhaUsuario)).Returns(_updateUsuarioRequest.SenhaUsuario);
            _usuarioMock.Setup(r => r.ObterPorId(_usuario.Id)).ReturnsAsync(_usuario);
            _usuarioResponse = await _atualizaService.Atualizar(_usuario.Id, _updateUsuarioRequest);
        }

        [Then("o sistema atualiza os dados daquele usuário")]
        public void ThenOSistemaAtualizaOsDadosDaqueleUsuario()
        {   
            _usuarioMock.Verify(r=>r.Atualizar(_usuario), Times.Once());
            Assert.Equal("Novo nome usuario", _usuarioResponse.Nome);
            Assert.Equal("NovaSenha@123", _updateUsuarioRequest.SenhaUsuario);
        }

        [When("o usuário submete um novo nome de usuário, porém o novo nome é pertencente a outro usuário")]
        public async Task WhenOUsuarioSubmeteUmNovoNomeDeUsuarioPoremONovoNomeEPertencenteAOutroUsuario()
        {
            try
            {
                _updateUsuarioRequest = new UpdateUsuarioRequest("Novo nome usuario", _faker.Internet.Email(), "NovaSenha@123", "NovaSenha@123");
                _usuario = new Usuario(new Email("teste@fiap.com"), new Senha("Senha@123"));
                _passwordMock.Setup(h => h.HashPassword(_updateUsuarioRequest.SenhaUsuario)).Returns(_updateUsuarioRequest.SenhaUsuario);
                _usuarioMock.Setup(h => h.VerificaNomeCadastradoParaAlteracao(_usuario.Id,_updateUsuarioRequest.NomeUsuario)).ReturnsAsync(true);
                _usuarioMock.Setup(r => r.ObterPorId(_usuario.Id)).ReturnsAsync(_usuario);
                _usuarioResponse = await _atualizaService.Atualizar(_usuario.Id, _updateUsuarioRequest);
            }
            catch(DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
            
        }

        [Then("o sistema não atualiza os dados do usuário")]
        public void ThenOSistemaNaoAtualizaOsDadosDoUsuario()
        {
            Assert.Null(_usuarioResponse.Nome);
            _usuarioMock.Verify(r => r.Atualizar(_usuario), Times.Never());
        }

        [When("o usuário submete uma nova senha válida")]
        public async Task WhenOUsuarioSubmeteUmaNovaSenhaValida()
        {
            _updateUsuarioRequest = new UpdateUsuarioRequest("Novo nome usuario", _faker.Internet.Email(), "NovaSenha@123", "NovaSenha@123");
            _usuario = new Usuario(new Email("teste@fiap.com"), new Senha("Senha@123"));
            _passwordMock.Setup(h => h.HashPassword(_updateUsuarioRequest.SenhaUsuario)).Returns(_updateUsuarioRequest.SenhaUsuario);
            _usuarioMock.Setup(r => r.ObterPorId(_usuario.Id)).ReturnsAsync(_usuario);
            _usuarioResponse = await _atualizaService.Atualizar(_usuario.Id, _updateUsuarioRequest);
        }

        [When("o usuário submete uma nova senha, porém com o tamanho de caracteres inválidos")]
        public async Task WhenOUsuarioSubmeteUmaNovaSenhaPoremComOTamanhoDeCaracteresInvalidos()
        {
            try
            {
                _updateUsuarioRequest.SenhaUsuario = "a@";
                _usuario = new Usuario(new Email("teste@fiap.com"), new Senha("Senha@123"));
                _usuarioMock.Setup(r => r.ObterPorId(_usuario.Id)).ReturnsAsync(_usuario);
                _usuarioResponse = await _atualizaService.Atualizar(_usuario.Id, _updateUsuarioRequest);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna mensagem de erro em relação ao nome do usuário já estar cadastrado")]
        public void ThenRetornaMensagemDeErroEmRelacaoAoNomeDoUsuarioJaEstarCadastrado()
        {
            string mensagemEsperada = MensagensDominio.NomeUsuarioJaCadastrado;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [Then("retorna mensagem de erro em relação ao tamanho de caracteres da senha ao usuário")]
        public void ThenRetornaMensagemDeErroEmRelacaoAoTamanhoDeCaracteresDaSenhaAoUsuario()
        {
            string mensagemEsperada = MensagensDominio.SenhaTamanhoInvalido;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }

        [When("o usuário submete um novo nome de usuário e senha válidos, porém o Id do usuário não é encontrado")]
        public async Task WhenOUsuarioSubmeteUmNovoNomeDeUsuarioESenhaValidosPoremOIdDoUsuarioNaoEEncontrado()
        {
            try
            {
                _updateUsuarioRequest = new UpdateUsuarioRequest("Novo nome usuario", _faker.Internet.Email(), "NovaSenha@123", "NovaSenha@123");
                _usuario = new Usuario(new Email("teste@fiap.com"), new Senha("Senha@123"));
                _usuarioResponse = await _atualizaService.Atualizar(_usuario.Id, _updateUsuarioRequest);
            }
            catch (DomainException ex)
            {
                _mensagemDeErroRetornada = ex.Message;
            }
        }

        [Then("retorna mensagem de erro de usuário não encontrado")]
        public void ThenRetornaMensagemDeErroDeUsuarioNaoEncontrado()
        {
            string mensagemEsperada = MensagensDominio.UsuarioNaoEncontrado;
            Assert.Equal(mensagemEsperada, _mensagemDeErroRetornada);
        }


    }
}
