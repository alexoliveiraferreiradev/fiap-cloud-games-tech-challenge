namespace FiapCloundGames.API.Infrastructure.Security
{
    public class JwtSettings
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpiracaoHoras { get; set; }
        public string Emissor { get; set; } = string.Empty;
        public string ValidoEm { get; set; } = string.Empty;
    }
}
