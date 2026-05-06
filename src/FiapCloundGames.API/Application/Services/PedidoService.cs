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
        private readonly ILogger<PedidoService> _logger;
        public PedidoService(IPedidoRepository pedidoRepository, IJogoRepository jogosRepository,
            IUsuarioRepository usuarioRepository, IBibliotecaService bibliotecaService, IMapper mapper, ILogger<PedidoService> logger)
        {
            _pedidoRepository = pedidoRepository;
            _jogoRepository = jogosRepository;
            _usuarioRepository = usuarioRepository;
            _bibliotecaService = bibliotecaService;
            _mapper = mapper;
            _logger = logger;
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
            _logger.LogInformation("Iniciando processamento de pedido para o usuário {UsuarioId} contendo {Quantidade} jogo(s).", usuarioId, jogosIds.Count);

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
                    _logger.LogWarning("Item ignorado no pedido: Jogo {JogoId} não encontrado no banco de dados.", idSolicitado);
                    errors.Add($"Jogo {idSolicitado} não encontrado.");
                    continue;
                }

                if (!jogo.Ativo)
                {
                    _logger.LogWarning("Item ignorado no pedido: Jogo {JogoId} ({NomeJogo}) está inativo.", jogo.Id, jogo.Nome.Valor);
                    errors.Add($"O jogo '{jogo.Nome.Valor}' não está disponível para venda.");
                    continue;
                }

                if (bibliotecaUsuario.Contains(jogo.Id))
                {
                    _logger.LogWarning("Item ignorado no pedido: Usuário {UsuarioId} já possui o jogo {JogoId}.", usuario.Id, jogo.Id);
                    errors.Add($"Você já possui o jogo '{jogo.Nome.Valor}' em sua biblioteca.");
                    continue;
                }

                pedido.AdicionarItem(jogo.Id, jogo.ObterPrecoAtual());
            }

            if (!pedido.Jogos.Any())
            {
                _logger.LogWarning("Pedido do usuário {UsuarioId} abortado. Nenhum dos {Quantidade} jogos solicitados era válido para compra.", usuario.Id, jogosIds.Count);
                throw new DomainException("Não foi possível realizar o pedido: " + string.Join(" ", errors));
            }
            pedido.FinalizarPedido();
            await _pedidoRepository.Adicionar(pedido);

            _logger.LogInformation("Pedido {PedidoId} criado com sucesso para o usuário {UsuarioId} com {QuantidadeItens} itens válidos.", pedido.Id, usuario.Id, pedido.Jogos.Count);

            var jogosPedidosId = pedido.Jogos.Select(x => x.JogoId).ToList();

            await _bibliotecaService.LiberarJogosAposPedido(usuario.Id, jogosPedidosId);
            var response = _mapper.Map<PedidoResponse>(pedido);
            
            response.MensagensInformativas = errors;

            _logger.LogInformation("Processo do pedido {PedidoId} concluído e jogos liberados na biblioteca do usuário {UsuarioId}.", pedido.Id, usuario.Id);

            return response;
        }


    }
}
