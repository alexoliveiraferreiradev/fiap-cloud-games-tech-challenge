using FiapCloundGames.API.Application.Dtos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }


        public async Task Adicionar(Usuario entity)
        {
            await _usuarioRepository.Adicionar(entity);
        }

        public async Task Atualizar(Usuario entity)
        {
            await _usuarioRepository.Atualizar(entity);
        }

        public async Task<Usuario> CriaAdministrador(CriaUsuarioRequest request, bool hasPermision, string token)
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
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioObrigatorio);
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

        public async Task<Usuario> CriaJogador(CriaUsuarioRequest request)
        {
            var usuario = new Usuario(request.Nome, request.Email, request.Senha, request.reSenha);
            await Adicionar(usuario);
            return usuario;
        }

        public async Task<Usuario> ObterPorId(Guid id)
        {
            return await _usuarioRepository.ObterPorId(id);
        }

        public Task<IEnumerable<Usuario>> ObterTodos()
        {
            throw new NotImplementedException();
        }

        public Task Remover(Usuario entity)
        {
            throw new NotImplementedException();
        }
    }
}
