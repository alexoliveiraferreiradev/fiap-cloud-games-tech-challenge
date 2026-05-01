using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IJogosRepository _jogoRepository;
        public PedidoService(IPedidoRepository pedidoRepository, IJogosRepository jogosRepository)
        {
            _pedidoRepository = pedidoRepository;
            _jogoRepository = jogosRepository;
        }
        public async Task<Pedido> RealizarPedido(Guid usuarioId, List<Guid> jogosIds)
        {
            var pedido = new Pedido(usuarioId);
            foreach (var jogoId in jogosIds)
            {
                var jogo = await _jogoRepository.ObterPorId(jogoId);
                if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
                if (!jogo.Ativo) throw new DomainException(MensagensDominio.JogoInvalido);
                pedido.AdicionarItem(jogo.Id, jogo.ObterPrecoAtual());
            }
            pedido.FinalizarPedido();
            await _pedidoRepository.Adicionar(pedido);
            return pedido;
        }
    }
}
