using AutoMapper;
using FiapCloudGames.Application.Dtos.Usuario;
using FiapCloudGames.Domain.Entities;
using FiapCloudGames.Domain.ValueObjects;

namespace FiapCloudGames.Application.Mappings
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioResponse>()
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeUsuario.Valor))
              .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailUsuario.Valor))
              .ForMember(dest=>dest.PerfilUsuario,opt=> opt.MapFrom(src=>src.Perfil))
              .ReverseMap()
              .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => new Nome(src.Nome)))
              .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => new Email(src.Email)));

            CreateMap<UsuarioResponse, ContaJogadorResponse>()
             .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
             .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.PerfilUsuario, opt => opt.MapFrom(src => src.PerfilUsuario))
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id ))
             .ForMember(dest => dest.DataAlteracao, opt => opt.MapFrom(src => src.DataAlteracao));

            CreateMap<Usuario, UsuarioAtualizadoResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeUsuario.Valor))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailUsuario.Valor))
                .ReverseMap()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => new Nome(src.Nome)))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => new Email(src.Email)));

        }
    }
}
