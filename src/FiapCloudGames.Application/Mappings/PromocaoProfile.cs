using AutoMapper;
using FiapCloudGames.Application.Dtos.Promocao;
using FiapCloudGames.Domain.Entities;

namespace FiapCloudGames.Application.Mappings
{
    public class PromocaoProfile : Profile
    {
        public PromocaoProfile()
        {

            CreateMap<Jogo, PromocaoResponse>()
                .ForMember(dest => dest.NomeJogo, opt => opt.MapFrom(src => src.Nome.Valor))
                .ForMember(dest => dest.DescricaoJogo, opt => opt.MapFrom(src => src.Descricao.Valor))
                .ForMember(dest=>dest.JogoId,opt=>opt.MapFrom(src=>src.Id))
                .ForMember(dest => dest.PromocaoId, opt => opt.MapFrom(src => src.Promocoes.FirstOrDefault(p => p.Ativo).Id))
                .ForMember(dest => dest.ValorPromocao, opt => opt.MapFrom(src => src.Promocoes.FirstOrDefault(p => p.Ativo).ValorPromocao.Valor))
                .ForMember(dest => dest.DataFim, opt => opt.MapFrom(src => src.Promocoes.FirstOrDefault(p => p.Ativo).Periodo.DataInicio))
                .ForMember(dest => dest.DataFim, opt => opt.MapFrom(src => src.Promocoes.FirstOrDefault(p => p.Ativo).Periodo.DataFim));


        }
    }
}
