namespace FiapCloundGames.API.Application.Dtos.Biblioteca
{
    public record BibliotecaResponse(
        Guid JogoId,
        string NomeJogo,
        string Descricao,
        string Genero,
        DateTime DataAquisicao
        );
}
