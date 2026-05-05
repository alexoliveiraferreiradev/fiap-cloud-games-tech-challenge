using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public class CriarJogoRequest
    {
        /// <summary>
        /// Nome do Jogo
        /// </summary>
        /// <example>Nome do Jogo</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 2)]
        public string Nome { get; set; }
        /// <summary>
        /// Descrição do Jogo
        /// </summary>
        /// <example>Descrição do Jogo</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(500, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string Descricao { get; set; }
        /// <summary>
        /// Preço do Jogo
        /// </summary>
        /// <example>Preço do Jogo</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal Preco { get; set; }
        /// <summary>
        /// Gênero do Jogo
        /// </summary>
        /// <example>Gênero do Jogo</example>
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public GeneroJogo Genero { get; set; }

        public CriarJogoRequest()
        {
        }

        public CriarJogoRequest(string nomeJogo, string descricaoJogo, decimal precoJogo, GeneroJogo generoJogo)
        {
            Nome = nomeJogo; Descricao = descricaoJogo; Preco = precoJogo; Genero = generoJogo;
        }
    }
}
