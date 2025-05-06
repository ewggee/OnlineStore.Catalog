using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OnlineStore.Catalog.Domain.Exceptions;
using System.Text.Json;

namespace OnlineStore.Catalog.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            var response = new ProblemDetails()
            {
                Title = exception.Message,
                Instance = context.HttpContext.Request.Path,
                Status = StatusCodes.Status500InternalServerError
            };

            switch (exception)
            {
                case { } when exception is EntityNotFoundException:
                {
                    response.Status = StatusCodes.Status404NotFound;
                    break;
                }
            }

            _logger.LogError($"Api method {response.Instance} finished with code {response.Status}. Error reponse: {JsonSerializer.Serialize(response)}");
            
            context.Result = new JsonResult(response) { StatusCode = response.Status };
        }
    }
}
