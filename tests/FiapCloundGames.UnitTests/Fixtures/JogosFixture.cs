using Bogus;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;
using FiapCloundGames.API.Domain.ValueObjects;

namespace FiapCloundGames.UnitTests.Fixtures
{
    public class JogosFixture
    {
        private readonly Faker _faker;
        private NomeJogo _nomeJogo;
        private Descricao _descricaoJogo;
        private Preco _precoJogo;
        private GeneroJogo _generoJogo;
        public JogosFixture()
        {
            _faker = new Faker();
            _nomeJogo = new NomeJogo(_faker.Random.String(10));
            _descricaoJogo = new Descricao(_faker.Random.String(50));
            _precoJogo = new Preco(_faker.Random.Decimal(10, 100));
            _generoJogo = _faker.PickRandom<GeneroJogo>();
        }

        public Jogo ObtemJogosComSucesso()
        {
            return new Jogo(_nomeJogo, _descricaoJogo, _precoJogo, _generoJogo);
        }
        public Jogo ObtemJogosParaPromocao()
        {
            return new Jogo(_nomeJogo, _descricaoJogo, new Preco(150.00m), _generoJogo);
        }
        public Jogo ObtemJogosInativo()
        {
            var jogos = new Jogo(_nomeJogo, _descricaoJogo, _precoJogo, _generoJogo);
            typeof(Jogo).GetProperty("Ativo").SetValue(jogos, false);
            return jogos;
        }
       
        public Jogo ObtemJogosGeneroInvalido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(100);
            var precoJogo = new Preco( _faker.Random.Decimal(10, 100));
            var generoJogo = (GeneroJogo)100;
            return new Jogo(_nomeJogo, _descricaoJogo, _precoJogo, generoJogo);
        }
    }
}
