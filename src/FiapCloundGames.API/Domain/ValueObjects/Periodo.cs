using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Periodo : ValueObject<Periodo>
    {
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }

        public Periodo(DateTime dataInicio,DateTime dataFinal)
        {
            if (dataFinal <= dataInicio) throw new DomainException(MensagensDominio.DataFimInvalida);
            DataInicio = dataInicio;
            DataFim = dataFinal;
        }
        public Periodo(DateTime dataFinal) : this(DateTime.UtcNow,dataFinal){ }

        protected override bool EqualsCore(Periodo other)
        {
            return DataInicio == other.DataInicio && DataFim == other.DataFim;
        }

        protected override int GetHashCodeCore()
        {
            return HashCode.Combine(DataInicio, DataFim);
        }
    }
}
