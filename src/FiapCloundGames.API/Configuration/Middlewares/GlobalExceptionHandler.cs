using FiapCloundGames.API.Configuration.Exceptions;
using FiapCloundGames.API.Domain.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FiapCloundGames.API.Configuration.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;   
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Ocorreu um erro: {Message}", exception.Message);
            if (exception is DomainException || exception is BusinessException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Erro de Negócio",
                    Detail = exception.Message 
                };

                httpContext.Response.StatusCode = problemDetails.Status.Value;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true;
            }
            var internalProblem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Erro Interno do Servidor",
                Detail = "Ocorreu um erro inesperado. Tente novamente mais tarde."
            };

            httpContext.Response.StatusCode = internalProblem.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(internalProblem, cancellationToken);

            return true; 
        }
    }
}
