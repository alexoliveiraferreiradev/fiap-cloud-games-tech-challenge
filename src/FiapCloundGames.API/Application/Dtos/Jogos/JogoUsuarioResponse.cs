using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public class JogoUsuarioResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoAtual { get; set; }
        public GeneroJogo Genero { get; set; }
        public bool TemDesconto{ get; set; }
    }
}
