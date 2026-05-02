using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace FiapCloundGames.API.Application.Dtos.Jogos
{
    public record UpdateJogosRequest
    {
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(100, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 2)]
        public string NovoNome { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        [StringLength(500, ErrorMessage = DataAnnotationMessage.ErroCaracteres, MinimumLength = 7)]
        public string NovaDescricao { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public decimal NovoPreco { get; set; }
        [Required(ErrorMessage = DataAnnotationMessage.ErroRequired)]
        public GeneroJogo NovoGenero { get; set; }

        public UpdateJogosRequest()
        {            
        }

        public UpdateJogosRequest(string novoNome, string novaDescricao, decimal novoPreco, GeneroJogo novoGenero)
        {
            NovoNome = novoNome; NovaDescricao = novaDescricao; NovoPreco = novoPreco; NovoGenero = novoGenero;
        }
    }
}
