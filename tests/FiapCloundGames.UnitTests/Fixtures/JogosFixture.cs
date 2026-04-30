using Bogus;
using FiapCloundGames.API.Domain.Entities;
using FiapCloundGames.API.Domain.Enum;

namespace FiapCloundGames.UnitTests.Fixtures
{
    public class JogosFixture
    {
        private readonly Faker _faker;
        public JogosFixture()
        {
            _faker = new Faker();
        }

        public Jogos ObtemJogosComSucesso()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = _faker.Random.Decimal(10, 100);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosParaPromocao()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = 150.00m;
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosInativo()
        {
            var jogos = new Jogos(_faker.Random.String(10), _faker.Random.String(50), _faker.Random.Decimal(10, 100), _faker.PickRandom<GeneroJogo>());
            typeof(Jogos).GetProperty("Ativo").SetValue(jogos, false);
            return jogos;
        }

        public Jogos ObtemJogosNomeNaoPreenchido()
        {
            var nomeJogo = string.Empty;
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = _faker.Random.Decimal(10, 100);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosDescricaoNaoPreenchido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = string.Empty;
            var precoJogo = _faker.Random.Decimal(10, 100);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosNomeInvalido()
        {
            var nomeJogo = _faker.Random.String(21);
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = _faker.Random.Decimal(10, 100);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosDescricaoInvalida()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(101);
            var precoJogo = _faker.Random.Decimal(10, 100);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosPrecoInvalido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(100);
            var precoJogo = _faker.Random.Decimal(-1, 0);
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosGeneroInvalido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(100);
            var precoJogo = _faker.Random.Decimal(10, 100);
            var generoJogo = (GeneroJogo)100;
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
    }
}
