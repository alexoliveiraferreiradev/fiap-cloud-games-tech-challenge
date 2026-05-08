using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.Enum;
using FiapCloudGames.Domain.Repositories;
using FiapCloudGames.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace FiapCloudGames.Infrastructure.Repository
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
            return await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(p => p.EmailUsuario.Valor.ToLower() == email.ToLower());
        }

        public async Task<bool> VerificaEmailCadastrado(string emailCadastrado)
        {
            return await _dbContext.Usuarios.AnyAsync(x => x.EmailUsuario.Valor.ToLower() == emailCadastrado.ToLower());
        }
              

        public async Task<bool> VerificaMaisDeUmAdminCadastrado()
        {
            return await _dbContext.Usuarios
         .CountAsync(x => x.Perfil == TipoUsuario.Administrador && x.Ativo) > 1;
        }

        public async Task<bool> VerificaNomeCadastrado(string nomeCadastrado)
        {
            return await _dbContext.Usuarios.AnyAsync(x => x.NomeUsuario.Valor.ToLower() == nomeCadastrado.ToLower());
        }

        public async Task<bool> VerificaNomeCadastradoParaAlteracao(Guid usuarioId, string nomeCadastrado)
        {
            return await _dbContext.Usuarios
                       .AnyAsync(x => x.EmailUsuario.Valor.ToLower() == nomeCadastrado.ToLower()
                        && x.Id != usuarioId);
        }
    }
}
