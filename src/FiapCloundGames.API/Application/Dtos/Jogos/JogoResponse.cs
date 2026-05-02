namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record JogoResponse
    (
        Guid Id,
        string Nome,
        string Descricao,
        decimal PrecoOriginal,
        decimal PrecoAtual
    )
    {
        public bool TemDesconto => PrecoAtual < PrecoOriginal;
    }
}
