using FiapCloudGames.Domain.Common;
using FiapCloudGames.Domain.Common.Exceptions;
using FiapCloudGames.Domain.Resources;
using FiapCloudGames.Domain.ValueObjects;

namespace FiapCloudGames.Domain.Entities
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
            DataAlteracao = DataCadastro;
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

        public void AtualizarPromocao(Preco novoPreco, DateTime novaDataFim)
        {
            if (ValorPromocao == novoPreco) return;
            ValorPromocao = novoPreco;
            if (Periodo.DataFim != novaDataFim)
            {
                Periodo = new Periodo(this.Periodo.DataInicio, novaDataFim);
            }
            DataAlteracao = DateTime.UtcNow;
        }

    }
}
