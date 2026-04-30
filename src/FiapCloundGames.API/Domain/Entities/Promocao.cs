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
            DataCadastro = DateTime.UtcNow; 
            ValidarEntidade();
        }
        protected Promocao() { }
        public Guid JogoId { get; private set; }
        public decimal Valor { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public DateTime DataAlteracao { get; private set; }

        public override void ValidarEntidade()
        {
            if (JogoId == Guid.Empty) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentValueFormat(Valor, MensagensDominio.ValorInvalido);
            if (DataFim <= DateTime.UtcNow) throw new DomainException(MensagensDominio.PromocaoDataFimInvalida);
        }

        public bool EstaValida() =>
            Ativo && DateTime.UtcNow >= DataInicio && DateTime.UtcNow <= DataFim;

        public void Desativar()
        {
            if (!EstaValida()) throw new DomainException(MensagensDominio.PromocaoInativa);
             Ativo = false;
            DataAlteracao = DateTime.UtcNow;    
        }

        public void AtualizarPrecoPromocional(decimal novoValor)
        {
            AssertionConcern.AssertArgumentValueFormat(novoValor, MensagensDominio.ValorInvalido);
            Valor = novoValor;
            DataAlteracao = DateTime.UtcNow;
        }

    }
}
