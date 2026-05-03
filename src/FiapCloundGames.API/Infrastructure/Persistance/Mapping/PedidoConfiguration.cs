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

            builder.OwnsMany(p => p.Jogos, i =>
            {
                i.ToTable("PedidoJogos");
                i.HasKey(x => x.Id);

                i.OwnsOne(x => x.ValorUnitario, v =>
                {                    
                    v.Property(p => p.Valor)
                        .HasColumnName("ValorUnitario") 
                        .HasPrecision(18, 2)
                        .IsRequired();
                });

                i.HasOne(x=>x.Jogo).WithMany().HasForeignKey(x => x.JogoId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Navigation(p => p.Jogos)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.HasOne(p => p.Usuario)
               .WithMany()            
               .HasForeignKey(p => p.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
