using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Jogos;
using FiapCloundGames.API.Application.Dtos.Promocao;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Configuration.Mapping
{
    public class JogoProfile : Profile
    {
        public JogoProfile()
        {
            CreateMap<Jogo, JogoResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome.Valor))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao.Valor))
                .ForMember(dest => dest.PrecoOriginal, opt => opt.MapFrom(src => src.PrecoBase.Valor))
                .ForMember(dest=>dest.Ativo,opt=>opt.MapFrom(src=>src.Ativo))
                .ForMember(dest=>dest.PrecoAtual,opt=>opt.MapFrom(src=>src.ObterPrecoAtual().Valor));

            CreateMap<JogoResponse, JogoUsuarioResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Descricao))
                .ForMember(dest => dest.PrecoAtual, opt => opt.MapFrom(src => src.PrecoAtual))
                .ForMember(dest => dest.TemDesconto, opt => opt.MapFrom(src => src.TemDesconto));

            CreateMap<Jogo,PromocaoResponse>()
                .ForMember(dest => dest.NomeJogo, opt => opt.MapFrom(src => src.Nome.Valor))
                .ForMember(dest => dest.DescricaoJogo, opt => opt.MapFrom(src => src.Descricao.Valor))
                .ForMember(dest => dest.ValorPromocao, opt => opt.MapFrom(src => src.ObterPrecoAtual().Valor));
        }
    }
}
