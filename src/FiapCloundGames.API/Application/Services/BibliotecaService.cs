using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

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
        public async Task AdicionaJogo(CriaBibliotecaRequest request)
        {
            var usuario = await _usuarioRepository.ObterPorId(request.usuarioId);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);

            var jogo = await _jogoRepository.ObterPorId(request.jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            if (!jogo.Ativo) throw new DomainException(MensagensDominio.JogoInvalido);

            var possuiJogo = await _bibliotecaRepository.VerificaSeUsuarioPossuiJogo(request.usuarioId, request.jogoId);
            if (possuiJogo) throw new DomainException(MensagensDominio.BibliotecaJogoRepetido);

            var bibliotecaItem = new Biblioteca(request.usuarioId, request.jogoId);
            await _bibliotecaRepository.Adicionar(bibliotecaItem);
        }

        public Task AtualizarDados(UpdateBibliotecaRequest updateBibliotecaRequest)
        {
            throw new NotImplementedException();
        }
    }
}
