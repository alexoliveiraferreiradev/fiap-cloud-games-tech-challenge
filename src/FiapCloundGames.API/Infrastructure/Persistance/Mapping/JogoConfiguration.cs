using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloundGames.API.Infrastructure.Persistance.Mapping
{
    public class JogoConfiguration : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("Jogos");
            
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Ativo).HasDefaultValue(true);
                       

            builder.OwnsOne(j => j.Nome, n =>
            {
                n.Property(p => p.Valor)
                    .HasColumnName("Nome") 
                    .HasMaxLength(100)
                    .IsRequired();
            });

            builder.OwnsOne(j => j.Descricao, d =>
            {
                d.Property(p => p.Valor)
                    .HasColumnName("Descricao")
                    .HasMaxLength(500)
                    .IsRequired();

            });

            builder.OwnsOne(j => j.PrecoBase, p =>
            {
                p.Property(v => v.Valor)
                    .HasColumnName("PrecoBase")
                    .HasPrecision(18, 2)
                    .IsRequired();

              
            });

            builder.Property(u => u.Genero)
                .IsRequired();

            builder.OwnsMany(j => j.Promocoes, p =>
            {
                p.ToTable("PromocaoJogos");
                p.HasKey(p => p.Id);
                p.Property(p => p.Id).ValueGeneratedNever();
                p.OwnsOne(x => x.Periodo, d =>
                {
                    d.Property(pp => pp.DataInicio)
                        .HasColumnName("DataInicio")
                        .IsRequired();

                    d.Property(pp => pp.DataFim)
                        .HasColumnName("DataFim")
                        .IsRequired();
                });

                p.OwnsOne(b => b.ValorPromocao, v =>
                {
                    v.Property(v => v.Valor)
                    .HasColumnName("ValorPromocao")
                    .HasPrecision(18, 2)
                    .IsRequired();   
                });

                p.WithOwner().HasForeignKey(x => x.JogoId);
            });

            builder.Navigation(p => p.Promocoes)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
