using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Promocao : EntityBase
    {
        public Promocao(Guid jogoId, decimal valorPromocao, DateTime dataFim)
        {
            JogoId = jogoId;    
            Valor = valorPromocao;
            Ativo = true;
            DataInicio = DateTime.UtcNow;
            DataFim = dataFim;
        }
        protected Promocao() { }
        public Guid JogoId { get; private set; }
        public decimal Valor { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }

        public override void ValidarEntidade()
        {
            if (JogoId == Guid.Empty) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentValueFormat(Valor, MensagensDominio.PromocaoValorInvalido);
            if (DataFim <= DateTime.UtcNow) throw new DomainException(MensagensDominio.PromocaoDataFimInvalida);
        }

        public bool EstaValida()=>
            Ativo && DateTime.UtcNow >= DataInicio && DateTime.UtcNow <= DataFim;

        public void Desativar() => Ativo = false;

        public void AtualizarPrecoPromocional(decimal novoValor)
        {
            AssertionConcern.AssertArgumentValueFormat(novoValor, MensagensDominio.PromocaoValorInvalido);
            Valor = novoValor;
        }

    }
}
