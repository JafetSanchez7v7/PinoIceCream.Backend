using Microsoft.EntityFrameworkCore;
using PinoHeladeria.Application.Exceptions;
using System.ComponentModel;
using System.Net;

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
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database update exception.");

                // Si el mensaje del Trigger contiene nuestra palabra clave 'vital'
                if (dbEx.InnerException?.Message.Contains("vital") == true)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new
                    {
                        Error = dbEx.InnerException.Message // Aquí viaja tu RAISERROR de SQL
                    });
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { Error = "Error de persistencia en base de datos." });
                }
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
