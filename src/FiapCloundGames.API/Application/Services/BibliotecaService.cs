using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FiapCloundGames.API.Application.Services
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IMapper _mapper;
        public BibliotecaService(IBibliotecaRepository bibliotecaRepository, IUsuarioRepository usuarioRepository,
            IJogoRepository jogosRepository, IMapper mapper)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _usuarioRepository = usuarioRepository;
            _jogoRepository = jogosRepository;
            _mapper = mapper;
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

                var possuiJogo = await _bibliotecaRepository.VerificaSeUsuarioPossuiJogo(usuario.Id, jogoId);
                if (possuiJogo) throw new DomainException(MensagensDominio.BibliotecaJogoRepetido);

                var bibliotecaItem = new Biblioteca(usuario.Id, jogo.Id);
                await _bibliotecaRepository.Adicionar(bibliotecaItem);
            }
        }

        public async Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId)
        {
            return await _bibliotecaRepository.VerificaSeUsuarioPossuiJogo(usuarioId, jogoId);
        }

        public async Task<PagedResult<BibliotecaResponse>> ObtemBibliotecaDoUsuarioPaginacao(Guid usuarioId, int pagina = 1, int tamanhoPagina = 10)
        {
            var totalRegistros = await _bibliotecaRepository.TotalJogosPorUsuario(usuarioId);
            var bibliotecaResponse = _mapper.Map<IEnumerable<BibliotecaResponse>>(await _bibliotecaRepository.ObterJogosPorUsuarioPaginacao(usuarioId, pagina, tamanhoPagina));
            return new PagedResult<BibliotecaResponse>(bibliotecaResponse, pagina, tamanhoPagina, totalRegistros);
        }

        public async Task<IEnumerable<Guid>> ObterIdsJogosDoUsuario(Guid usuarioId)
        {
            return await _bibliotecaRepository.ObterIdsJogosDoUsuario(usuarioId);
        }
    }
}
