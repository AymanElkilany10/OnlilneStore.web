using Domain.Exceptions;
using Shared.ErrorModels;

namespace OnlineStore.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
                if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                {
                    await HandlingNotFoundEndPointAsync(context);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandlingErrorAsync(context, ex);
            }
        }

        private static async Task HandlingErrorAsync(HttpContext context, Exception ex)
        {
            //context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                ErrorMessage = ex.Message
            };

            context.Response.StatusCode = ex switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                BadRequestException => StatusCodes.Status400BadRequest,
                UnAuthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException => HandlingValidationExceptionAsync((ValidationException)ex, response),
                _ => StatusCodes.Status500InternalServerError
            };

            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

        private static async Task HandlingNotFoundEndPointAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorDetails
            {
                StatusCode = StatusCodes.Status404NotFound,
                ErrorMessage = $"End point {context.Request.Path} is Not Found"
            };

            await context.Response.WriteAsJsonAsync(response);
        }

        private static int HandlingValidationExceptionAsync(ValidationException ex, ErrorDetails response)
        {
            response.Errors = ex.Errors;
            return StatusCodes.Status400BadRequest;
        }
    }
}
