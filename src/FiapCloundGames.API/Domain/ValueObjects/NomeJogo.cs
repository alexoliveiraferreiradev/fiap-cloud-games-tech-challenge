using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class NomeJogo : ValueObject<NomeJogo>
    {
        public string Valor { get; }

        public NomeJogo(string nomeJogo)
        {
            AssertionConcern.AssertArgumentEmpty(nomeJogo, MensagensDominio.JogoNomeObrigatorio);
            AssertionConcern.AssertArgumentLength(nomeJogo, 3, 100, MensagensDominio.JogoTamanhoNomeInvalido);
            Valor = nomeJogo;
        }
        protected override bool EqualsCore(NomeJogo other)
        {
            return Valor == other.Valor;    
        }

        protected override int GetHashCodeCore()
        {
            return Valor.GetHashCode();
        }
    }
}
