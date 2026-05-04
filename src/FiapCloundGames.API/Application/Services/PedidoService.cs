using AutoMapper;
using FiapCloundGames.API.Application.Services.Interfaces;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Repositories;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IBibliotecaService _bibliotecaService;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private List<string> errors;
        public PedidoService(IPedidoRepository pedidoRepository, IJogoRepository jogosRepository,
            IUsuarioRepository usuarioRepository, IBibliotecaService bibliotecaService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _jogoRepository = jogosRepository;
            _usuarioRepository = usuarioRepository;
            _bibliotecaService = bibliotecaService;
            errors = new List<string>();
            _mapper = mapper;
        }

        public IEnumerable<string> ObtemErrosDoPedido()
        {
            return errors;
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
            errors = new List<string>();
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);

            var jogosNoBanco = await _jogoRepository.ObterJogosPorIds(jogosIds);
            var bibliotecaUsuario = await _bibliotecaService.ObterIdsJogosDoUsuario(usuarioId);

            var pedido = new Pedido(usuario.Id);
            foreach(var idSolicitado in jogosIds)
            {
                var jogo = jogosNoBanco.FirstOrDefault(j => j.Id == idSolicitado);

                if (jogo == null)
                {
                    errors.Add($"Jogo {idSolicitado} não encontrado.");
                    continue;
                }

                if (!jogo.Ativo)
                {
                    errors.Add($"O jogo '{jogo.Nome.Valor}' não está disponível para venda.");
                    continue;
                }

                if (bibliotecaUsuario.Contains(jogo.Id))
                {
                    errors.Add($"Você já possui o jogo '{jogo.Nome.Valor}' em sua biblioteca.");
                    continue;
                }

                pedido.AdicionarItem(jogo.Id, jogo.ObterPrecoAtual());
            }

            if (!pedido.Jogos.Any())
            {
                throw new DomainException("Não foi possível realizar o pedido: " + string.Join(" ", errors));
            }
            pedido.FinalizarPedido();
            await _pedidoRepository.Adicionar(pedido);
            await _bibliotecaService.LiberarJogosAposPedido(usuario.Id,jogosIds);

            return pedido;
        }


    }
}
