using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Persistance.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){            
        }
    }
}
