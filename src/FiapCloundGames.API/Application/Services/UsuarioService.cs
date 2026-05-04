using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IPasswordHasher _passwordHasher;
        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }

        public async Task<UsuarioResponse> PromoverParaAdmin(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.PromoverPerfil(usuario);
            await _usuarioRepository.Atualizar(usuario);
            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<UsuarioResponse> RebaixarParaJogador(Guid idUsuarioRebaixar, Guid idOperador)
        {
            if (idUsuarioRebaixar == idOperador) throw new DomainException(MensagensDominio.OperacaoRebaixarInvalida);

            var usuario = await _usuarioRepository.ObterPorId(idUsuarioRebaixar);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            if (usuario.Perfil.Equals(TipoUsuario.Jogador)) throw new DomainException(MensagensDominio.UsuarioPerfilRebaixarInvalido);

            usuario.RebaixarPerfil();

            await _usuarioRepository.Atualizar(usuario);
            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<UsuarioResponse> CadastrarUsuario(CriaUsuarioRequest request)
        {
            var nomeVO = new Nome(request.Nome);
            var emailVO = new Email(request.Email);
            if (await _usuarioRepository.VerificaEmailCadastrado(request.Email)) throw new DomainException(MensagensDominio.EmailJaCadastrado);
            if (await _usuarioRepository.VerificaNomeCadastrado(request.Nome)) throw new DomainException(MensagensDominio.NomeUsuarioJaCadastrado);
            ValidaSenhas(request.Senha, request.ConfirmacaoSenha);

            var senhaCifrada = _passwordHasher.HashPassword(request.Senha);
            var senhaCifradaVO = new Senha(senhaCifrada);

            var usuario = new Usuario(nomeVO, emailVO, senhaCifradaVO);
            await _usuarioRepository.Adicionar(usuario);
            return _mapper.Map<UsuarioResponse>(usuario);
        }

        private static void ValidaSenhas(string senhaRequest, string confirmacaoSenhaRequest)
        {
            AssertionConcern.AssertArgumentEquals(senhaRequest, confirmacaoSenhaRequest, MensagensDominio.UsuarioSenhaConfirmacaoDiferente);
        }

        public async Task<UsuarioResponse> AtualizarUsuario(Guid id, UpdateUsuarioRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);

            if (await _usuarioRepository.VerificaEmailCadastradoParaAlteracao(id, request.EmailUsuario)) throw new DomainException(MensagensDominio.EmailJaCadastrado);
            if (await _usuarioRepository.VerificaNomeCadastradoParaAlteracao(id, request.NomeUsuario)) throw new DomainException(MensagensDominio.NomeUsuarioJaCadastrado);

            ValidaSenhas(request.SenhaUsuario, request.ConfirmacaoSenha);

            var novaSenhaCriptografa = _passwordHasher.HashPassword(request.SenhaUsuario);
            var novoUsuarioVO = new Nome(request.NomeUsuario);
            var novoEmailUsuarioVO = new Email(request.EmailUsuario);
            var novaSenhaUsuarioVO = new Senha(novaSenhaCriptografa);

            usuario.Atualizar(novoUsuarioVO, novoEmailUsuarioVO, novaSenhaUsuarioVO);
            await _usuarioRepository.Atualizar(usuario);
            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task<UsuarioResponse> ObterPorId(Guid id)
        {
            return _mapper.Map<UsuarioResponse>(await _usuarioRepository.ObterPorId(id));
        }

        public async Task<IEnumerable<UsuarioResponse>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<UsuarioResponse>>(await _usuarioRepository.ObterTodos());
        }

        public async Task Desativar(DesativaUsuarioRequest deletaUsuarioRequest, Guid idOperador)
        {
            if (idOperador == deletaUsuarioRequest.Id) throw new DomainException(MensagensDominio.OperacaoDesativarInvalida);
            var admin = await _usuarioRepository.ObterPorId(idOperador);
            if (admin == null) throw new DomainException(MensagensDominio.AdminNaoEncontrado);
            var usuario = await _usuarioRepository.ObterPorId(deletaUsuarioRequest.Id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.Desativar(deletaUsuarioRequest.MotivoDelecao);
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task DesativarConta(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            if (usuario.Perfil == TipoUsuario.Administrador)
            {
                var existeOutroAdmin = await _usuarioRepository.VerificaMaisDeUmAdminCadastrado();
                if (!existeOutroAdmin)
                {
                    throw new DomainException(MensagensDominio.OperacaoDesativarAdminInvalida);
                }
            }
            usuario.DesativarConta();
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<UsuarioResponse> Autenticar(LoginRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorEmail(request.Email);
            if (usuario == null) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);
            bool senhaValida = _passwordHasher.VerifyPassword(request.Senha, usuario.Senha.Hash);
            if (!senhaValida) throw new DomainException(MensagensDominio.CrendenciasInvalidas);
            return _mapper.Map<UsuarioResponse>(usuario);
        }

        public async Task Reativar(Guid id)
        {
            var usuario = await _usuarioRepository.ObterPorId(id);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            usuario.Reativar();
            await _usuarioRepository.Atualizar(usuario);
        }

        public async Task<UsuarioResponse?> ObterPorEmail(string emailUsuario)
        {
            var email = new Email(emailUsuario);
            return _mapper.Map<UsuarioResponse>(await _usuarioRepository.ObterPorEmail(email.Valor));
        }

        public async Task<bool> VerificaAdminCadastrado()
        {
            return await _usuarioRepository.VerificaMaisDeUmAdminCadastrado();
        }
    }
}
