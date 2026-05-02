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

        public JogoResponse()
        {            
        }

        public JogoResponse(Guid jogoId, string nomeJogo, string descricaoJogo, decimal precoOriginal, decimal precoAtual)
        {
            Id = jogoId; Nome = nomeJogo; Descricao = descricaoJogo; PrecoOriginal = precoOriginal; PrecoAtual = precoAtual;
        }

    }
}
