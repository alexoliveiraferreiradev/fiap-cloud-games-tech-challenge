using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Configuration.Mappings
{
    public class BibliotecaProfile : Profile
    {
        public BibliotecaProfile()
        {
            CreateMap<Biblioteca, BibliotecaResponse>()
                .ForMember(dest => dest.NomeJogo, opt => opt.MapFrom(src => src.Jogo.Nome.Valor))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.Jogo.Descricao.Valor))
                .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Jogo.Genero))
                .ForMember(dest => dest.DataAquisicao, opt => opt.MapFrom(src => src.DataCadastro));
                
        }
    }
}
