using FiapCloundGames.API.Domain.Common;
using FiapCloundGames.API.Domain.Resources;

namespace FiapCloundGames.API.Domain.ValueObjects
{
    public class Descricao : ValueObject<Descricao>
    {
        public string Valor { get; set; }

        public Descricao(string descricao)
        {
            AssertionConcern.AssertArgumentEmpty(descricao, MensagensDominio.JogoDescricaoObrigatoria);
            AssertionConcern.AssertArgumentLength(descricao, 5, 500, MensagensDominio.JogoDescricaoTamanhoInvalido);
            Valor = descricao;
        }

        protected override bool EqualsCore(Descricao other)
        {
            return Valor == other.Valor;    
        }

        protected override int GetHashCodeCore()
        {
            return Valor.GetHashCode();
        }
    }
}
