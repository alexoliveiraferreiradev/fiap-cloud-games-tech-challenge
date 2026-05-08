using FiapCloudGames.Domain.Enum;
using FiapCloudGames.Domain.Resources;
using System.ComponentModel.DataAnnotations;

namespace FiapCloudGames.Application.Dtos.Jogos
{
    public record UpdateJogoRequest
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

        public UpdateJogoRequest()
        {            
        }

        public UpdateJogoRequest(string novoNome, string novaDescricao, decimal novoPreco, GeneroJogo novoGenero)
        {
            NovoNome = novoNome; NovaDescricao = novaDescricao; NovoPreco = novoPreco; NovoGenero = novoGenero;
        }
    }
}
