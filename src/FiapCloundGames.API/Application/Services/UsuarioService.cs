using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

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

        public async Task<Usuario> CadastrarAdministrador(CriaUsuarioRequest request, bool hasPermision, string token)
        {
            ValidaSenhas(request.Senha, request.ConfirmacaoSenha);
            var emailUsuarioValueObject = new Email(request.Email);
            var nomeUsuarioValueObject = new Nome(request.Nome);
            var senhaUsuarioValueObject = new Senha(request.Senha);
            var usuario = new Usuario(nomeUsuarioValueObject, emailUsuarioValueObject, senhaUsuarioValueObject);
            if (!ValidaPermissoesAdministrador(hasPermision, token)) throw new DomainException(MensagensDominio.PermissaoNegadaCriarAdministrador);
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


        private bool ValidaPermissoesAdministrador(bool hasPermision, string token)
        {
            if (!hasPermision || !IsValidToken(token))
            {
                throw new DomainException(MensagensDominio.PermissaoNegadaCriarAdministrador);
            }
            return true;
        }

        private bool IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }
            return true;
        }

        public async Task<Usuario> CadastrarUsuario(CriaUsuarioRequest request)
        {
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

        public Task<IEnumerable<Usuario>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public async Task Desativar(DeleteUsuarioRequest deletaUsuarioRequest)
        {
            var usuario = await _usuarioRepository.ObterPorId(deletaUsuarioRequest.id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.Desativar(deletaUsuarioRequest.motivoDelecao);
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<Usuario> Autenticar(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(request.emailUsuario);
            if (usuario == null) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);
            bool senhaValida = _passwordHasher.VerifyPassword(request.senhaUsuario, usuario.Senha.Hash);
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
    }
}
