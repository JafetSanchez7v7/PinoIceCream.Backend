using PinoHeladeria.Application.Exceptions;
using System.ComponentModel;

namespace PinoHeladeria.API.MIddleWares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
            catch(ApiException ex)
            {
                _logger.LogWarning(ex, "A handled API exception has occurred.");
                context.Response.StatusCode = ex.StatusCode;
                await context.Response.WriteAsJsonAsync(new
                {
                    Error = ex.Message,
                    details = ex is ErrorValidationException validationEx ? validationEx.Errors : null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new {

                    Error = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
