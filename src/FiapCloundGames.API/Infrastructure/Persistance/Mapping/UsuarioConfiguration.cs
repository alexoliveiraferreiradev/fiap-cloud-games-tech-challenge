using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FiapCloundGames.API.Infrastructure.Persistance.Mapping
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        private readonly PasswordHasher _passwordHasher;
        public UsuarioConfiguration()
        {
            _passwordHasher = new PasswordHasher();
        }
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(p => p.Id);
            builder.OwnsOne(u => u.NomeUsuario, n =>
            {
                n.Property(p => p.Valor).HasColumnName("Nome").IsRequired().HasMaxLength(50);
            });

            builder.OwnsOne(u => u.EmailUsuario, e =>
            {
                e.Property(p => p.Valor).HasColumnName("Email").IsRequired().HasMaxLength(100);
            });

            builder.OwnsOne(u => u.Senha, s =>
            {
                s.Property(p => p.Hash).HasColumnName("Senha").IsRequired().HasMaxLength(60);
            });

            builder.Property(u => u.MotivoDesativacao)
                .IsRequired(false);            

            var adminId = Guid.Parse("AEA0B4F3-D220-4C8D-ABA8-D868BE7CA593");
            var senhaHash = _passwordHasher.HashPassword("SenhaAdmin@123");
            builder.HasData(new
            {
                Id = adminId,
                Ativo = true,
                Perfil = TipoUsuario.Administrador,
                Nome_Valor = "Admin Sistema",
                Email_Valor = "admin@fiapcloundgames.com.br",
                Senha_Hash = senhaHash,
                DataCadastro = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            });

            var jogadorId = Guid.Parse("E99C8CAF-7067-49B4-8EEF-B0D7ED801033");
            senhaHash = _passwordHasher.HashPassword("SenhaJogador@123");
            builder.HasData(new
            {
                Id = jogadorId,
                Ativo = true,
                Perfil = TipoUsuario.Jogador,
                Nome_Valor = "Jogador 1",
                Email_Valor = "jogador1@fiapcloundgames.com.br",
                Senha_Hash = senhaHash,
                DataCadastro = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            });

            jogadorId = Guid.Parse("BC359373-699B-4152-B63C-4FBF5A509AAD");
            senhaHash = _passwordHasher.HashPassword("SenhaJogador@1234");
            builder.HasData(new
            {
                Id = jogadorId,
                Ativo = true,
                Perfil = TipoUsuario.Jogador,
                Nome_Valor = "Jogador 2",
                Email_Valor = "jogador2@fiapcloundgames.com.br",
                Senha_Hash = senhaHash,
                DataCadastro = DateTime.UtcNow,
                DataAlteracao = DateTime.UtcNow
            });
        }
    }
}
