using FiapCloudGames.Domain.Enum;

namespace FiapCloudGames.Application.Dtos.Jogos
{
    public class JogoFiltroRequest
    {
        public int Pagina { get; set; } = 1;
        public int Tamanho { get; set; } = 10;
        public string? Busca { get; set; }
        public GeneroJogo? Genero { get; set; }
        public bool? ApenasPromovidos { get; set; } 
    }
}
