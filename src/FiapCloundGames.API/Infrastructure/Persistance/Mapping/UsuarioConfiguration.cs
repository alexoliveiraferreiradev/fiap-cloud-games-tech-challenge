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
            builder.Property(u => u.NomeUsuario.Valor)
                .IsRequired()
                .HasColumnType("varchar(20)");

            builder.Property(u => u.EmailUsuario.Valor)
                .IsRequired()
                .HasColumnType("varchar(40)");

            builder.Property(u => u.Senha.Hash)
                .IsRequired()
                .HasColumnType("nvarchar(60)");

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
                Email_Endereco = "admin@fiapcloundgames.com.br",
                Senha = senhaHash,
                DataCadastro = DateTime.UtcNow
            });
        }
    }
}
