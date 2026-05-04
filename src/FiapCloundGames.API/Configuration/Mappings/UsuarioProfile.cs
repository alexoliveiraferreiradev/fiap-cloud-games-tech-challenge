using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Usuario;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Configuration.Mapping
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

            CreateMap<Usuario, UsuarioAtualizadoResponse>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeUsuario.Valor))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailUsuario.Valor))
                .ReverseMap()
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => new Nome(src.Nome)))
                .ForMember(dest => dest.EmailUsuario, opt => opt.MapFrom(src => new Email(src.Email)));

        }
    }
}
