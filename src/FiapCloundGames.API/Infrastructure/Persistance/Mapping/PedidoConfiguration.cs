using FiapCloundGames.API.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloundGames.API.Infrastructure.Persistance.Mapping
{
    public class PedidoConfiguration : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("Pedidos");
            builder.HasKey(p => p.Id);

            builder.OwnsOne(p => p.ValorTotal, v =>
            {
                v.Property(p => p.Valor).HasColumnName("PrecoTotal").HasDefaultValue(0).HasPrecision(18, 2);
            });

            builder.HasOne(p => p.Usuario)
               .WithMany()            
               .HasForeignKey(p => p.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
