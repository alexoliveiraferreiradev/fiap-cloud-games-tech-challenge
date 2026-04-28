using FiapCloundGames.API.Application.Dtos;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Entities;
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
    }
}
