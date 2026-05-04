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

                n.HasData(new { JogoId = Guid.Parse("FD0D99FE-0196-414B-B103-5A371C5E804B"), Valor = "Euro Truck Simulator 2" });
                n.HasData(new { JogoId = Guid.Parse("8138C73C-FC4C-42C9-A316-FF7874069510"), Valor = "Oxygen Not Included" });
                n.HasData(new { JogoId = Guid.Parse("4EE09524-2C09-4283-A633-6E8DDD0B8D15"), Valor = "Crimson Desert" });
                n.HasData(new { JogoId = Guid.Parse("0B01CDF5-5421-4DFA-A40A-1DF159B24571"), Valor = "18 Wheels of Steel: Haulin'" });
                n.HasData(new { JogoId = Guid.Parse("52A756F6-392C-47E8-AD07-4C1EE260ED2F"), Valor = "Hades" });
                n.HasData(new { JogoId = Guid.Parse("942B8596-A092-45EB-8302-F9EABC1637DC"), Valor = "Red Dead Redemption 2" });
                n.HasData(new { JogoId = Guid.Parse("137AC0A2-D703-4FD7-A4FD-67B1A7BD21C4"), Valor = "American Truck Simulator" });
                n.HasData(new { JogoId = Guid.Parse("58564834-2F6E-4D98-B3B3-F3C2CE2DA0B0"), Valor = "Elden Ring" });
                n.HasData(new { JogoId = Guid.Parse("4362AAB0-06E5-455F-A19E-D4F585BDE400"), Valor = "Factorio" });
                n.HasData(new { JogoId = Guid.Parse("FB1D25BA-9E7F-44BE-A716-F03914EE8430"), Valor = "Stardew Valley" });
                n.HasData(new { JogoId = Guid.Parse("A79A3378-3FE4-4BBA-94BE-440B097694AC"), Valor = "The Witcher 3: Wild Hunt" });
            });

            builder.OwnsOne(j => j.Descricao, d =>
            {
                d.Property(p => p.Valor)
                    .HasColumnName("Descricao")
                    .HasMaxLength(500)
                    .IsRequired();

                d.HasData(new { JogoId = Guid.Parse("FD0D99FE-0196-414B-B103-5A371C5E804B"), Valor = "Viaje pela Europa como o rei da estrada, um caminhoneiro que entrega cargas importantes em distâncias impressionantes." });
                d.HasData(new { JogoId = Guid.Parse("8138C73C-FC4C-42C9-A316-FF7874069510"), Valor = "Um simulador de sobrevivência espacial onde você gerencia seus colonos e os ajuda a cavar, construir e manter uma base asteroide subterrânea." });
                d.HasData(new { JogoId = Guid.Parse("4EE09524-2C09-4283-A633-6E8DDD0B8D15"), Valor = "Um jogo de ação e aventura em mundo aberto ambientado em um continente de Pywel, focado em sobrevivência e mercenários." });
                d.HasData(new { JogoId = Guid.Parse("0B01CDF5-5421-4DFA-A40A-1DF159B24571"), Valor = "Gerencie sua própria empresa de caminhões e domine as estradas norte-americanas neste clássico da simulação." });
                d.HasData(new { JogoId = Guid.Parse("52A756F6-392C-47E8-AD07-4C1EE260ED2F"), Valor = "Desafie o deus dos mortos enquanto você abre caminho para fora do Submundo neste dungeon crawler roguelike." });
                d.HasData(new { JogoId = Guid.Parse("942B8596-A092-45EB-8302-F9EABC1637DC"), Valor = "Red Dead Redemption 2 é uma história épica de honra e lealdade no alvorecer dos tempos modernos." });
                d.HasData(new { JogoId = Guid.Parse("137AC0A2-D703-4FD7-A4FD-67B1A7BD21C4"), Valor = "Experimente a lendária liberdade americana dirigindo caminhões icônicos através de paisagens deslumbrantes e marcos históricos dos Estados Unidos." });
                d.HasData(new { JogoId = Guid.Parse("58564834-2F6E-4D98-B3B3-F3C2CE2DA0B0"), Valor = "Levante-se, Maculado, e seja guiado pela graça para portar o poder do Anel Príncipio e se tornar um Lorde Príncipio nas Terras Intermédias." });
                d.HasData(new { JogoId = Guid.Parse("4362AAB0-06E5-455F-A19E-D4F585BDE400"), Valor = "Construa, automatize e gerencie fábricas complexas em um planeta alienígena infinito para lançar um foguete ao espaço." });
                d.HasData(new { JogoId = Guid.Parse("FB1D25BA-9E7F-44BE-A716-F03914EE8430"), Valor = "Você herdou a antiga fazenda do seu avô. Com ferramentas de segunda mão e algumas moedas, você parte para começar sua nova vida." });
                d.HasData(new { JogoId = Guid.Parse("A79A3378-3FE4-4BBA-94BE-440B097694AC"), Valor = "Torne-se um caçador de monstros profissional e embarque em uma aventura épica para encontrar a criança da profecia em um mundo aberto vasto." });
            });

            builder.OwnsOne(j => j.PrecoBase, p =>
            {
                p.Property(v => v.Valor)
                    .HasColumnName("PrecoBase")
                    .HasPrecision(18, 2)
                    .IsRequired();

                p.HasData(new { JogoId = Guid.Parse("FD0D99FE-0196-414B-B103-5A371C5E804B"), Valor = 61.99m });
                p.HasData(new { JogoId = Guid.Parse("8138C73C-FC4C-42C9-A316-FF7874069510"), Valor = 45.99m });
                p.HasData(new { JogoId = Guid.Parse("4EE09524-2C09-4283-A633-6E8DDD0B8D15"), Valor = 249.90m });
                p.HasData(new { JogoId = Guid.Parse("0B01CDF5-5421-4DFA-A40A-1DF159B24571"), Valor = 19.90m });
                p.HasData(new { JogoId = Guid.Parse("52A756F6-392C-47E8-AD07-4C1EE260ED2F"), Valor = 73.99m });
                p.HasData(new { JogoId = Guid.Parse("942B8596-A092-45EB-8302-F9EABC1637DC"), Valor = 199.99m });
                p.HasData(new { JogoId = Guid.Parse("137AC0A2-D703-4FD7-A4FD-67B1A7BD21C4"), Valor = 61.99m });
                p.HasData(new { JogoId = Guid.Parse("58564834-2F6E-4D98-B3B3-F3C2CE2DA0B0"), Valor = 229.90m });
                p.HasData(new { JogoId = Guid.Parse("4362AAB0-06E5-455F-A19E-D4F585BDE400"), Valor = 125.00m });
                p.HasData(new { JogoId = Guid.Parse("FB1D25BA-9E7F-44BE-A716-F03914EE8430"), Valor = 24.99m });
                p.HasData(new { JogoId = Guid.Parse("A79A3378-3FE4-4BBA-94BE-440B097694AC"), Valor = 129.99m });
            });

            builder.HasData(
            new
            {
                Id = Guid.Parse("FD0D99FE-0196-414B-B103-5A371C5E804B"),
                Ativo = true,
                Genero = GeneroJogo.Simulacao,
                DataCadastro = new DateTime(2026, 5, 2),
                DataAlteracao = new DateTime(2026, 5, 2)
            },
            new
            {
                Id = Guid.Parse("8138C73C-FC4C-42C9-A316-FF7874069510"),
                Ativo = true,
                Genero = GeneroJogo.Aventura,
                DataCadastro = new DateTime(2026, 5, 2),
                DataAlteracao = new DateTime(2026, 5, 2)
            },
            new
            {
                Id = Guid.Parse("4EE09524-2C09-4283-A633-6E8DDD0B8D15"),
                Ativo = true,
                Genero = GeneroJogo.Acao,
                DataCadastro = new DateTime(2026, 5, 2),
                DataAlteracao = new DateTime(2026, 5, 2)
            },
            new
            {
                Id = Guid.Parse("0B01CDF5-5421-4DFA-A40A-1DF159B24571"),
                Ativo = true,
                Genero = GeneroJogo.Simulacao,
                DataCadastro = new DateTime(2026, 5, 2),
                DataAlteracao = new DateTime(2026, 5, 2)
            },
            new
            {
                Id = Guid.Parse("52A756F6-392C-47E8-AD07-4C1EE260ED2F"),
                Ativo = true,
                Genero = GeneroJogo.RPG,
                DataCadastro = new DateTime(2026, 5, 2),
                DataAlteracao = new DateTime(2026, 5, 2)
            },
             new
             {
                 Id = Guid.Parse("942B8596-A092-45EB-8302-F9EABC1637DC"),
                 Ativo = true,
                 Genero = GeneroJogo.MundoAberto,
                 DataCadastro = new DateTime(2026, 5, 2),
                 DataAlteracao = new DateTime(2026, 5, 2)
             },
             new
             {
                 Id = Guid.Parse("137AC0A2-D703-4FD7-A4FD-67B1A7BD21C4"),
                 Ativo = true,
                 Genero = GeneroJogo.Simulacao,
                 DataCadastro = new DateTime(2026, 5, 2),
                 DataAlteracao = new DateTime(2026, 5, 2)
             },
              new
              {
                  Id = Guid.Parse("58564834-2F6E-4D98-B3B3-F3C2CE2DA0B0"),
                  Ativo = true,
                  Genero = GeneroJogo.RPG,
                  DataCadastro = new DateTime(2026, 5, 2),
                  DataAlteracao = new DateTime(2026, 5, 2)
              },
              new
              {
                  Id = Guid.Parse("4362AAB0-06E5-455F-A19E-D4F585BDE400"),
                  Ativo = true,
                  Genero = GeneroJogo.Estrategia,
                  DataCadastro = new DateTime(2026, 5, 2),
                  DataAlteracao = new DateTime(2026, 5, 2)
              },
              new
              {
                  Id = Guid.Parse("FB1D25BA-9E7F-44BE-A716-F03914EE8430"),
                  Ativo = true,
                  Genero = GeneroJogo.Indie,
                  DataCadastro = new DateTime(2026, 5, 2),
                  DataAlteracao = new DateTime(2026, 5, 2)
              },
              new
              {
                  Id = Guid.Parse("A79A3378-3FE4-4BBA-94BE-440B097694AC"),
                  Ativo = true,
                  Genero = GeneroJogo.RPG,
                  DataCadastro = new DateTime(2026, 5, 2),
                  DataAlteracao = new DateTime(2026, 5, 2)
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
