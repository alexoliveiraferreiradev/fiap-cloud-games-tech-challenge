using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Pedido;
using FiapCloundGames.API.Domain.Entities;

namespace FiapCloundGames.API.Configuration.Mapping
{
    public class PedidoProfile : Profile
    {
        public PedidoProfile()
        {
            CreateMap<Pedido, PedidoResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest=>dest.UsuarioId,opt=> opt.MapFrom(src=>src.UsuarioId))
            .ForMember(dest => dest.DataPedido, opt => opt.MapFrom(src => src.DataCadastro))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Jogos))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal.Valor));
            CreateMap<PedidoJogo, PedidoItemResponse>()
            .ForMember(dest => dest.NomeJogo, opt => opt.MapFrom(src => src.Jogo.Nome.Valor))
            .ForMember(dest => dest.PrecoOriginal, opt => opt.MapFrom(src => src.Jogo.PrecoBase.Valor))
            .ForMember(dest => dest.PrecoPago, opt => opt.MapFrom(src => src.Jogo.ObterPrecoAtual().Valor));

        }
    }
}
