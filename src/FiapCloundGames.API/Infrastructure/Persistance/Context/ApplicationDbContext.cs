using FiapCloundGames.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapCloundGames.API.Infrastructure.Persistance.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){            
        }

        public DbSet<Usuario> Usuarios { get; set; }    
        public DbSet<Jogo> Jogos { get; set; }  
        public DbSet<Biblioteca> Bibliotecas { get; set; }
        public DbSet<Pedido> Pedidos {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
