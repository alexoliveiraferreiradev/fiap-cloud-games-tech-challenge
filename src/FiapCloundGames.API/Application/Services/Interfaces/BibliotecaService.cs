using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services.Interfaces
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
        public async Task AdicionaJogo(Guid usuarioId, Guid jogoId)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);

            if (usuario == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            if (!usuario.Ativo) throw new DomainException(MensagensDominio.UsuarioInativo);

            var jogo = await _jogoRepository.ObterPorId(jogoId);
            if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            if (!jogo.Ativo) throw new DomainException(MensagensDominio.JogoInvalido);

            var biblioteca = new Biblioteca(usuarioId, jogoId);
            biblioteca.AdicionaJogo(jogo.Nome, jogo.Descricao, jogo.Genero);
            await _bibliotecaRepository.Adicionar(biblioteca);
        }

        public Task AtualizarDados(UpdateBibliotecaRequest updateBibliotecaRequest)
        {
            throw new NotImplementedException();
        }
    }
}
