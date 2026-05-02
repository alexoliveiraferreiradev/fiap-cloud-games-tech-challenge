using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Application.Services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogosRepository _jogoRepository;
        public BibliotecaService(IBibliotecaRepository bibliotecaRepository, IUsuarioRepository usuarioRepository,
            IJogosRepository jogosRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogosRepository;
        }
        public async Task LiberarJogosAposPedido(Guid usuarioId, List<Guid> jogosIds)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);

            foreach (var jogoId in jogosIds)
            {
                var jogo = await _jogoRepository.ObterPorId(jogoId);
                if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
                if (!jogo.Ativo) throw new DomainException(MensagensDominio.JogoInvalido);

                var possuiJogo = await _bibliotecaRepository.VerificaSeUsuarioPossuiJogo(usuario.Id, jogo.Id);
                if (possuiJogo) throw new DomainException(MensagensDominio.BibliotecaJogoRepetido);

                var bibliotecaItem = new Biblioteca(usuario.Id, jogo.Id);
                await _bibliotecaRepository.Adicionar(bibliotecaItem);
            }
        }

        public async Task<IEnumerable<BibliotecaResponse>> ObterJogosPorUsuario(Guid usuarioId)
        {
            return await _bibliotecaRepository.ObterJogosPorUsuario(usuarioId);
        }
    }
}
