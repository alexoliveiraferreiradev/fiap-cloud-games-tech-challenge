using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Configuration.Mapping
{
    public class PromocaoProfile : Profile
    {
        public PromocaoProfile()
        {
            CreateMap<Promocao, PromocaoResponse>()
                .ForMember(dest=>dest.PromocaoId,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest=>dest.JogoId,opt=>opt.MapFrom(src=>src.JogoId))
                .ForMember(dest=>dest.ValorPromocao,opt=>opt.MapFrom(src=>src.ValorPromocao.Valor))
                .ForMember(dest=>dest.DataFim,opt=>opt.MapFrom(src=>src.Periodo.DataFim));

            CreateMap<Jogo, PromocaoResponse>()
                .ForMember(dest => dest.NomeJogo, opt => opt.MapFrom(src => src.Nome.Valor))
                .ForMember(dest => dest.DescricaoJogo, opt => opt.MapFrom(src => src.Descricao.Valor));
        }
    }
}
