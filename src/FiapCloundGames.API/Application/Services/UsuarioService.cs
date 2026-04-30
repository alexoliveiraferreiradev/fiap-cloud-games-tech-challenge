using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Common.Interfaces;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
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


        public async Task Adicionar(Usuario entity)
        {
            await _usuarioRepository.Adicionar(entity);
        }

        public async Task Atualizar(Usuario entity)
        {
            await _usuarioRepository.Atualizar(entity);
        }

        public async Task<Usuario> CadastrarAdministrador(CriaUsuarioRequest request, bool hasPermision, string token)
        {
            var usuario = new Usuario(request.Nome, request.Email, request.Senha, request.reSenha);
            if (!ValidaPermissoesAdministrador(hasPermision, token)) throw new DomainException(MensagensDominio.PermissaoNegadaCriarAdministrador);
            usuario.PromoverPerfil(usuario);
            await Adicionar(usuario);
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

        public async Task<Usuario> CadastrarJogador(CriaUsuarioRequest request)
        {
            ValidaSenhas(request.Senha,request.reSenha);
            var senhaCifrada = _passwordHasher.HashPassword(request.Senha);
            var confirmacaoSenha = senhaCifrada;
            var usuario = new Usuario(request.Nome, request.Email, senhaCifrada, confirmacaoSenha);
            await Adicionar(usuario);
            return usuario;
        }

        private static void ValidaSenhas(string senhaRequest, string confirmacaoSenhaRequest)
        {
            AssertionConcern.AssertArgumentEquals(senhaRequest, confirmacaoSenhaRequest, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
        }

        public async Task AtualizarUsuario(Guid id, UpdateUsuarioRequest request)
        {
            ValidaSenhas(request.senhaUsuario, request.reSenhaUsuario);
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);

            var novaSenhaCriptografa = _passwordHasher.HashPassword(request.senhaUsuario);
            var confirmacaoSenha = novaSenhaCriptografa;
            usuario.Atualizar(request.nomeUsuario, request.emailUsuario, novaSenhaCriptografa, confirmacaoSenha);
            await Atualizar(usuario);
        }

        public async Task<Usuario> ObterPorId(Guid id)
        {
            return await _usuarioRepository.ObterPorId(id);
        }

        public Task<IEnumerable<Usuario>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public async Task DesativarUsuario(DeleteUsuarioRequest deletaUsuarioRequest)
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
            bool senhaValida = _passwordHasher.VerifyPassword(request.senhaUsuario, usuario.Senha);
            if (!senhaValida) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            return usuario;
        }

    }
}
