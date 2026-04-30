namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public class JogoResponse
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal PrecoOriginal { get; set; }
        public decimal PrecoAtual { get; set; }
        public bool TemDesconto => PrecoAtual < PrecoOriginal;
    }
}
