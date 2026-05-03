using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Usuario> PromoverParaAdmin(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);            
            usuario.PromoverPerfil(usuario);
            await _usuarioRepository.Atualizar(usuario);
            return usuario;
        }

        public async Task RebaixarPerfil(Guid idUsuarioRebaixar, Guid idAdminExecutor)
        {
            var adminExecutor = await _usuarioRepository.ObterPorId(idAdminExecutor);
            if (adminExecutor == null || !adminExecutor.Perfil.Equals(TipoUsuario.Administrador)) throw new DomainException(MensagensDominio.PermissaoNegadaCriarAdministrador);

            var usuario = await _usuarioRepository.ObterPorId(idUsuarioRebaixar);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            if (usuario.Perfil.Equals(TipoUsuario.Jogador)) throw new DomainException(MensagensDominio.UsuarioPerfilRebaixarInvalido);

            usuario.RebaixarPerfil();

            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<Usuario> CadastrarUsuario(CriaUsuarioRequest request)
        {
            if (await _usuarioRepository.VerificaEmailCadastrado(request.Email)) throw new DomainException(MensagensDominio.EmailJaCadastrado);
            if (await _usuarioRepository.VerificaNomeCadastrado(request.Nome)) throw new DomainException(MensagensDominio.NomeUsuarioJaCadastrado);

            ValidaSenhas(request.Senha, request.ConfirmacaoSenha);

            var senhaCifrada = _passwordHasher.HashPassword(request.Senha);

            var nomeVO = new Nome(request.Nome);
            var emailVO = new Email(request.Email);
            var senhaCifradaVO = new Senha(senhaCifrada);

            var usuario = new Usuario(nomeVO, emailVO, senhaCifradaVO);
            await _usuarioRepository.Adicionar(usuario);
            return usuario;
        }

        private static void ValidaSenhas(string senhaRequest, string confirmacaoSenhaRequest)
        {
            AssertionConcern.AssertArgumentEquals(senhaRequest, confirmacaoSenhaRequest, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
        }

        public async Task AtualizarUsuario(Guid id, UpdateUsuarioRequest request)
        {
            ValidaSenhas(request.SenhaUsuario, request.ConfirmacaoSenha);
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);

            var novaSenhaCriptografa = _passwordHasher.HashPassword(request.SenhaUsuario);
            var novoUsuarioVO = new Nome(request.NomeUsuario);
            var novoEmailUsuarioVO = new Email(request.EmailUsuario);
            var novaSenhaUsuarioVO = new Senha(novaSenhaCriptografa);

            usuario.Atualizar(novoUsuarioVO, novoEmailUsuarioVO, novaSenhaUsuarioVO);
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<Usuario> ObterPorId(Guid id)
        {
            return await _usuarioRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<Usuario>> ObterTodos()
        {
            return await _usuarioRepository.ObterTodos();
        }

        public async Task Desativar(DeleteUsuarioRequest deletaUsuarioRequest)
        {
            var usuario = await _usuarioRepository.ObterPorId(deletaUsuarioRequest.Id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.Desativar(deletaUsuarioRequest.MotivoDelecao);
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<Usuario> Autenticar(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(request.Email);
            if (usuario == null) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);
            bool senhaValida = _passwordHasher.VerifyPassword(request.Senha, usuario.Senha.Hash);
            if (!senhaValida) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            return usuario;
        }

        public async Task Reativar(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.Reativar();
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<Usuario?> ObterPorEmail(string emailUsuario)
        {
            var email = new Email(emailUsuario);
            return await _usuarioRepository.ObterPorEmail(email.Valor);
        }
    }
}
