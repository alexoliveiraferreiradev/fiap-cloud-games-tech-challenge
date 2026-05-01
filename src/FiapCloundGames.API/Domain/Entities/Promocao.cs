using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.API.Domain.Entities
{
    public class Promocao : EntityBase
    {
        public Promocao(Guid jogoId, Preco valorPromocao, Periodo periodo)
        {
            JogoId = jogoId;
            ValorPromocao = valorPromocao;
            Ativo = true;
            Periodo = periodo;
            DataCadastro = DateTime.UtcNow;
            ValidarEntidade();
        }
        protected Promocao() { }
        public Guid JogoId { get; private set; }
        public Preco ValorPromocao { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime DataCadastro { get; private set; }
        public Periodo Periodo { get; private set; }
        public DateTime DataAlteracao { get; private set; }

        protected override void ValidarEntidade()
        {
            if (JogoId == Guid.Empty) throw new DomainException(MensagensDominio.JogoNaoEncontrado);
            AssertionConcern.AssertArgumentNotNull(Periodo, MensagensDominio.PeriodoObrigatorio);
        }

        public bool EstaValida() =>
            Ativo && DateTime.UtcNow >= Periodo.DataInicio && DateTime.UtcNow <= Periodo.DataFim;

        public void Desativar()
        {
            if (!EstaValida()) throw new DomainException(MensagensDominio.PromocaoInativa);
            Ativo = false;
            DataAlteracao = DateTime.UtcNow;
        }

        public void AtualizarPrecoPromocional(Preco novoValor)
        {
            ValorPromocao = novoValor;
            DataAlteracao = DateTime.UtcNow;
        }

    }
}
