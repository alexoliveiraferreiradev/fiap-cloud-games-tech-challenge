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
            builder.OwnsOne(j => j.Nome, n =>
            {
                n.Property(p => p.Valor).HasColumnName("Nome").IsRequired().HasMaxLength(40);
            });

            builder.OwnsOne(j => j.Descricao, d =>
            {
                d.Property(p => p.Valor).HasColumnName("Descricao").IsRequired().HasMaxLength(100);
            });

            builder.OwnsOne(j => j.PrecoBase, p =>
            {
                p.Property(v => v.Valor).HasColumnName("PrecoBase").HasDefaultValue(0).HasPrecision(18, 2);
            });

            builder.Property(u => u.Genero)
                .IsRequired();

            builder.HasData(
            new
            {
                Id = Guid.Parse("67873138-062A-4433-875C-62CD08D7959B"),
                Ativo = true,
                Genero = GeneroJogo.Simulacao,
                Nome_Valor = "Euro Truck Simulator 2",
                Descricao_Valor = "Viaje pela Europa como o rei da estrada, um caminhoneiro que entrega cargas importantes em distâncias impressionantes.",
                PrecoBase_Valor = 61.99m
            },
            new
            {
                Id = Guid.Parse("4A6F126C-9B3D-4A2D-BA4C-8F8D5A1E2F3G"),
                Ativo = true,
                Genero = GeneroJogo.Aventura,
                Nome_Valor = "Oxygen Not Included",
                Descricao_Valor = "Um simulador de sobrevivência espacial onde você gerencia seus colonos e os ajuda a cavar, construir e manter uma base asteroide subterrânea.",
                PrecoBase_Valor = 45.99m
            },
            new
            {
                Id = Guid.Parse("A1B2C3D4-E5F6-4G7H-8I9J-K0L1M2N3O4P5"),
                Ativo = true,
                Genero = GeneroJogo.Acao,
                Nome_Valor = "Crimson Desert",
                Descricao_Valor = "Um jogo de ação e aventura em mundo aberto ambientado em um continente de Pywel, focado em sobrevivência e mercenários.",
                PrecoBase_Valor = 249.90m
            },
            new
            {
                Id = Guid.Parse("D8E7F6C5-B4A3-2109-8765-43210FEDCBA9"),
                Ativo = true,
                Genero = GeneroJogo.Simulacao,
                Nome_Valor = "18 Wheels of Steel: Haulin'",
                Descricao_Valor = "Gerencie sua própria empresa de caminhões e domine as estradas norte-americanas neste clássico da simulação.",
                PrecoBase_Valor = 19.90m
            },
            new
            {
                Id = Guid.Parse("F5E4D3C2-B1A0-9876-5432-10FEDCBA9876"),
                Ativo = true,
                Genero = GeneroJogo.RPG,
                Nome_Valor = "Hades",
                Descricao_Valor = "Desafie o deus dos mortos enquanto você abre caminho para fora do Submundo neste dungeon crawler roguelike.",
                PrecoBase_Valor = 73.99m
            },
             new
             {
                 Id = Guid.Parse("40f017c9-87e3-4627-9e1e-d92a8d132141"),
                 Ativo = true,
                 Genero = GeneroJogo.MundoAberto,
                 Nome_Valor = "Red Dead Redemption 2",
                 Descricao_Valor = "Red Dead Redemption 2 é uma história épica de honra e lealdade no alvorecer dos tempos modernos.",
                 PrecoBase_Valor = 199.99m
             },
             new
             {
                 Id = Guid.Parse("E3F2D1C0-B9A8-4756-B123-C4D5E6F7A8B9"),
                 Ativo = true,
                 Genero = GeneroJogo.Simulacao,
                 Nome_Valor = "American Truck Simulator",
                 Descricao_Valor = "Experimente a lendária liberdade americana dirigindo caminhões icônicos através de paisagens deslumbrantes e marcos históricos dos Estados Unidos.",
                 PrecoBase_Valor = 61.99m
             },
              new
              {
                  Id = Guid.Parse("B7C8D9E0-F1A2-4B3C-9D4E-5F6A7B8C9D0E"),
                  Ativo = true,
                  Genero = GeneroJogo.RPG,
                  Nome_Valor = "Elden Ring",
                  Descricao_Valor = "Levante-se, Maculado, e seja guiado pela graça para portar o poder do Anel Príncipio e se tornar um Lorde Príncipio nas Terras Intermédias.",
                  PrecoBase_Valor = 229.90m
              },
              new
              {
                  Id = Guid.Parse("12345678-ABCD-EFGH-IJKL-9876543210MN"),
                  Ativo = true,
                  Genero = GeneroJogo.Estrategia,
                  Nome_Valor = "Factorio",
                  Descricao_Valor = "Construa, automatize e gerencie fábricas complexas em um planeta alienígena infinito para lançar um foguete ao espaço.",
                  PrecoBase_Valor = 125.00m
              },
              new
              {
                  Id = Guid.Parse("A1B2C3D4-9999-4444-8888-FFEEBBAA7766"),
                  Ativo = true,
                  Genero = GeneroJogo.Indie,
                  Nome_Valor = "Stardew Valley",
                  Descricao_Valor = "Você herdou a antiga fazenda do seu avô. Com ferramentas de segunda mão e algumas moedas, você parte para começar sua nova vida.",
                  PrecoBase_Valor = 24.99m
              },
              new
              {
                  Id = Guid.Parse("C9B8A7D6-E5F4-3210-GHIJ-K0L1M2N3O4P5"),
                  Ativo = true,
                  Genero = GeneroJogo.RPG,
                  Nome_Valor = "The Witcher 3: Wild Hunt",
                  Descricao_Valor = "Torne-se um caçador de monstros profissional e embarque em uma aventura épica para encontrar a criança da profecia em um mundo aberto vasto.",
                  PrecoBase_Valor = 129.99m
              }
        );
        }
    }
}
