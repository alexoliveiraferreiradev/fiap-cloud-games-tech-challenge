using AutoMapper;
using FiapCloundGames.API.Application.Dtos.Pedido;
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
        public PedidoService(IPedidoRepository pedidoRepository, IJogoRepository jogosRepository,
            IUsuarioRepository usuarioRepository, IBibliotecaService bibliotecaService, IMapper mapper)
        {
            _pedidoRepository = pedidoRepository;
            _jogoRepository = jogosRepository;
            _usuarioRepository = usuarioRepository;
            _bibliotecaService = bibliotecaService;
            _mapper = mapper;
        }      

        public async Task<IEnumerable<PedidoResponse>> ObtemHistoricoPorUsuario(Guid usuarioId)
        {
            return _mapper.Map<IEnumerable<PedidoResponse>>( await _pedidoRepository.ObtemHistoricoPorUsuario(usuarioId));
        }

        public async Task<PedidoResponse> ObterPedidoPorId(Guid id)
        {
            return  _mapper.Map<PedidoResponse>( await _pedidoRepository.ObterPorId(id));              
        }

        public async Task<PedidoResponse> RealizarPedido(Guid usuarioId, List<Guid> jogosIds)
        {
            var usuario = await _usuarioRepository.ObterPorId(usuarioId);
            if (usuario == null) throw new DomainException(MensagensDominio.UsuarioNaoEncontrado);

            var jogosNoBanco = await _jogoRepository.ObterJogosPorIds(jogosIds);
            var bibliotecaUsuario = await _bibliotecaService.ObterIdsJogosDoUsuario(usuarioId);

            var pedido = new Pedido(usuario.Id);
            var errors = new List<string>();
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
            var response = _mapper.Map<PedidoResponse>(pedido);
            response.MensagensInformativas = errors;
            return response;
        }


    }
}
