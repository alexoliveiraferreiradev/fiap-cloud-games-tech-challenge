using FiapCloundGames.API.Application.Dtos.Biblioteca;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;
using FiapCloundGames.API.Infrastructure.Repository;

namespace FiapCloundGames.API.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IBibliotecaService _bibliotecaService;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IJogosRepository _jogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        public PedidoService(IPedidoRepository pedidoRepository, IJogosRepository jogosRepository,
            IUsuarioRepository usuarioRepository, IBibliotecaService bibliotecaService)
        {
            _pedidoRepository = pedidoRepository;
            _jogoRepository = jogosRepository;
            _usuarioRepository = usuarioRepository;
            _bibliotecaService = bibliotecaService;
        }

        public async Task<IEnumerable<Pedido>> ObtemHistoricoPorUsuario(Guid usuarioId)
        {
            return await _pedidoRepository.ObtemHistoricoPorUsuario(usuarioId);
        }

        public async Task<Pedido> ObterPedidoPorId(Guid id)
        {
            return await _pedidoRepository.ObterPorId(id);              
        }

        public async Task<Pedido> RealizarPedido(Guid usuarioId, List<Guid> jogosIds)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);
            var pedido = new Pedido(usuario.Id);
            foreach (var jogoId in jogosIds)
            {
                var jogo = await _jogoRepository.ObterPorId(jogoId);
                if (jogo == null) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
                if (!jogo.Ativo) throw new DomainException(MensagensDominio.JogoInvalido);
                pedido.AdicionarItem(jogo.Id, jogo.ObterPrecoAtual());
            }
            pedido.FinalizarPedido();
            await _pedidoRepository.Adicionar(pedido);

            await _bibliotecaService.LiberarJogosAposPedido(usuario.Id,jogosIds);
            return pedido;
        }
    }
}
