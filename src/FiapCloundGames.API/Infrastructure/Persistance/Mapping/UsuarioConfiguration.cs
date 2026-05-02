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
                n.Property(p => p.Valor).HasColumnName("Nome").IsRequired().HasMaxLength(20);
            });

            builder.OwnsOne(u => u.EmailUsuario, e =>
            {
                e.Property(p => p.Valor).HasColumnName("Email").IsRequired().HasMaxLength(40);
            });

            builder.OwnsOne(u => u.Senha, s =>
            {
                s.Property(p => p.Hash).HasColumnName("Senha").IsRequired().HasMaxLength(60);
            });

            builder.Property(u => u.MotivoDesativacao)
                .IsRequired(false);

            builder.Property(u => u.DataCadastro)
                .HasColumnType("datetime");

            builder.Property(u => u.DataAlteracao)
                .HasColumnType("datetime");

            var adminId = Guid.Parse("83237EA1-B29B-41DE-B2C6-D905C8CC41C3");
            var senhaHash = _passwordHasher.HashPassword("SenhaAdmin@123");
            builder.HasData(new
            {
                Id = adminId,
                Ativo = true,
                Perfil = TipoUsuario.Administrador,
                Nome_Valor = "Administrador Sistema",
                Email_Valor = "admin@fiapcloundgames.com.br",
                Senha_Hash = senhaHash,
                DataCadastro = DateTime.UtcNow
            });
        }
    }
}
