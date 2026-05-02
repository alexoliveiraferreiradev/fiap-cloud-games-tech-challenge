using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance.Context;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Repository
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UsuarioRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext; 
        }

        public async Task<Usuario?> ObterPorEmail(string email)
        {
            return await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(p => p.EmailUsuario.Valor == email);
        }
    }
}
