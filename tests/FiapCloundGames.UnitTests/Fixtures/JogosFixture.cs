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

        public Jogos ObtemJogosComSucesso()
        {
            return new Jogos(_nomeJogo, _descricaoJogo, _precoJogo, _generoJogo);
        }
        public Jogos ObtemJogosParaPromocao()
        {
            return new Jogos(_nomeJogo, _descricaoJogo, new Preco(150.00m), _generoJogo);
        }
        public Jogos ObtemJogosInativo()
        {
            var jogos = new Jogos(_nomeJogo, _descricaoJogo, _precoJogo, _generoJogo);
            typeof(Jogos).GetProperty("Ativo").SetValue(jogos, false);
            return jogos;
        }

        public Jogos ObtemJogosNomeNaoPreenchido()
        {
            var nomeJogo = string.Empty;
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = new  Preco(_faker.Random.Decimal(10, 100));
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosDescricaoNaoPreenchido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = string.Empty;
            var precoJogo = new Preco( _faker.Random.Decimal(10, 100));
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosNomeInvalido()
        {
            var nomeJogo = _faker.Random.String(21);
            var descricaoJogo = _faker.Random.String(50);
            var precoJogo = new Preco( _faker.Random.Decimal(10, 100));
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosDescricaoInvalida()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(101);
            var precoJogo = new Preco(_faker.Random.Decimal(10, 100));
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosPrecoInvalido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(100);
            var precoJogo = new Preco(_faker.Random.Decimal(-1, 0));
            GeneroJogo generoJogo = _faker.PickRandom<GeneroJogo>();
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
        public Jogos ObtemJogosGeneroInvalido()
        {
            var nomeJogo = _faker.Random.String(10);
            var descricaoJogo = _faker.Random.String(100);
            var precoJogo = new Preco( _faker.Random.Decimal(10, 100));
            var generoJogo = (GeneroJogo)100;
            return new Jogos(nomeJogo, descricaoJogo, precoJogo, generoJogo);
        }
    }
}
