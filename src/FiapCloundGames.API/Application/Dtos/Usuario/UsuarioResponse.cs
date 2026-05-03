using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Usuario
{
    public class UsuarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public TipoUsuario PerfilUsuario {  get; set; }
        public DateTime DataAlteracao { get; set; }
        public MotivoExclusao? MotivoDesativacao { get; private set; }
    }
}
