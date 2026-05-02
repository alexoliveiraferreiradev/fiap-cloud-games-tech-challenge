using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public class BibliotecaRepository : Repository<Biblioteca>, IBibliotecaRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BibliotecaRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IEnumerable<BibliotecaResponse>> ObterJogosPorUsuario(Guid usuarioId)
        {
            return await _dbContext.Bibliotecas
                     .AsNoTracking() 
                    .Where(b => b.UsuarioId == usuarioId)
                    .Select(b => new BibliotecaResponse
                    {
                        JogoId = b.JogoId,
                        Descricao = b.Jogo.Descricao.Valor,
                        DataAquisicao = b.DataCadastro,
                        Genero = b.Jogo.Genero.ToString()
                    }).ToListAsync();
        }

        public async Task<bool> VerificaSeUsuarioPossuiJogo(Guid usuarioId, Guid jogoId)
        {
            return await _dbContext.Bibliotecas.AnyAsync(p =>p.UsuarioId == usuarioId && p.JogoId == jogoId);
        }
    }
}
