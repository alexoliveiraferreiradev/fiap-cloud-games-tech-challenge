using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Infrastructure.Persistance;
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
            return await _dbContext.Usuarios.AsNoTracking().FirstOrDefaultAsync(p => p.EmailUsuario.Valor.ToLower() == email.ToLower());
        }

        public async Task<bool> VerificaEmailCadastrado(string emailCadastrado)
        {
            return await _dbContext.Usuarios.AnyAsync(x => x.EmailUsuario.Valor.ToLower() == emailCadastrado.ToLower());
        }

        public async Task<bool> VerificaEmailCadastradoParaAlteracao(Guid usuarioId, string emailUsuario)
        {
            return await _dbContext.Usuarios
                        .AnyAsync(x => x.EmailUsuario.Valor.ToLower() == emailUsuario.ToLower()
                     && x.Id != usuarioId);
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
