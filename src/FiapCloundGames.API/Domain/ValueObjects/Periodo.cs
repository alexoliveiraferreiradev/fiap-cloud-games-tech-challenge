using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Common.Exceptions;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Periodo : ValueObject<Periodo>
    {
        public DateTime DataInicio { get; }
        public DateTime DataFim { get; }

        public Periodo(DateTime dataFinal)
        {
            if (dataFinal <= DateTime.UtcNow) throw new DomainException(MensagensDominio.DataFimInvalida);
            DataInicio = DateTime.UtcNow;
            DataFim = dataFinal;
        }
        public Periodo() { }

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
