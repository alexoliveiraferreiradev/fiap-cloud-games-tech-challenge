using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services
{
    public class JogosService : IJogosService
    {
        private readonly IJogosRepository _jogoRepository;
        public JogosService(IJogosRepository jogoRepository) {
            _jogoRepository = jogoRepository;   
        }
        public async Task<Jogos> CriaJogo(CriarJogoRequest request)
        {
            var jogos = new Jogos(request.Nome, request.Descricao, request.Preco, request.Genero);
            await Adicionar(jogos);
            return jogos;   
        }

        private async Task Adicionar(Jogos jogos)
        {
            await _jogoRepository.Adicionar(jogos); 
        }

        public async Task AtualizarJogo(Guid id, UpdateJogosRequest updateJogosRequest)
        {
            var jogo = await _jogoRepository.ObterPorId(id);            
            if(jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.Atualizar(updateJogosRequest.novoNome,updateJogosRequest.novaDescricao, updateJogosRequest.novoPreco, updateJogosRequest.novoGenero);
            await _jogoRepository.Atualizar(jogo);  
        }

        public async Task Desativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);     
            if(jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            jogo.Desativar();
        }

        public async Task Reativar(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorId(jogoId);
            jogo.Reativar();    
        }
    }
}
