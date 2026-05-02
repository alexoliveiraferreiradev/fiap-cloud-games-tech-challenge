using FiapCloundGames.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloundGames.API.Infrastructure.Persistance.Mapping
{
    public class BibliotecaConfiguration : IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.ToTable("Bibliotecas");
            builder.HasKey(x => x.Id);
            builder.HasIndex(p => new { p.UsuarioId, p.JogoId })
            .IsUnique();
            builder.HasOne(b => b.Usuario)
                .WithMany()
                .HasForeignKey(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b=>b.Jogo)
                .WithMany()
                .HasForeignKey(b=>b.JogoId)
                .OnDelete(DeleteBehavior.Restrict);  
        }
    }
}
