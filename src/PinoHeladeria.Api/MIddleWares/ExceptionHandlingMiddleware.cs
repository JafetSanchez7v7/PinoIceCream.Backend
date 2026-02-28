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
            // este es un catch para mis excepciones personalizadas que heredan de ApiException,
            // esto me permite manejar mejor y evitar bloques de codigo gigantes en el servicio o controladores mounstro
            catch (ApiException ex)
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
                var currentEx = (Exception)dbEx;
                bool isVitalError = false;
                string errorMessage = "Error de persistencia en base de datos.";

                // Aqui yo tengo un trigger en Bd pero el problema es que el orm que uso osea EFC 
                // no me permite acceder a ese mensaje de error que yo lanzo desde el trigger, entonces lo que hago
                // es recorrer la cadena de excepciones internas buscando ese mensaje
                //tambien le especifico a efc en el Contexto de Bd que tengo un trigger asi si me lanza un error vital como modificar
                // al admin o eliminarlo me va a lanzar ese error y yo lo voy a poder detectar aqui y devolver un mensaje mas amigable al cliente
                while (currentEx != null)
                {
                    if (currentEx.Message.ToLower().Contains("vital"))
                    {
                        isVitalError = true;
                        errorMessage = currentEx.Message; 
                        break; 
                    }
                    currentEx = currentEx.InnerException;
                }

                if (isVitalError)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await context.Response.WriteAsJsonAsync(new { Error = errorMessage });
                }
                else
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { Error = "Errod" });
                }
            }
            //esto es por si es un 500 ya de humo
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
