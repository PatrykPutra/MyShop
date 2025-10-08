
using MyShop.Exceptions;
using System.Text.Json;

namespace MyShop.Middleware
{
    public class ErrorHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next) // Spoko global error handling
        {
            try
            {
                await next.Invoke(context);
            }
            catch(NotFoundException ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.StatusCode = 404;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (UnauthorizedRequestException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (InvalidCredentialsException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = "Something went wrong" }));
            }
        }
    }
}
