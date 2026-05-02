namespace FiapCloundGames.API.Domain.Resources
{
    public class DataAnnotationMessage : Attribute
    {
        public const string ErroCaracteres = "O campo {0} precisa ter entre {2} e {1} caracteres";

        public const string ErroRequired = "O campo {0} é obrigatório";

        public const string ErroFormatoEmail = "O campo {0} está em formato incorreto";
    }
}
