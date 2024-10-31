using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace DogsHouse.API.Middleware
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
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning(ex, "Validation error occurred");
                await HandleExceptionAsync(context, ex, StatusCodes.Status400BadRequest);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "JSON parsing error occurred");
                await HandleExceptionAsync(context, new Exception("Invalid JSON format"), StatusCodes.Status400BadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred");
                await HandleExceptionAsync(context, ex, StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, int statusCode)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                error = exception.Message,
                statusCode
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
