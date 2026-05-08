namespace FiapCloundGames.API.Domain.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException()
        {

        }
        public BusinessException(string message) : base(message)
        {
        }

        public BusinessException(string message, string paramName) : base($"{message} (Parâmetro: {paramName})")
        {
        }
    }
}
