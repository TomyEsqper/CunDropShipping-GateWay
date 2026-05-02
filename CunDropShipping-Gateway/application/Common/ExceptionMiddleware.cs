using System.Net; // Necesario para HttpStatusCode
using System.Text.Json; // Para convertir nuestra respuesta a JSON

namespace CunDropShipping_Gateway.application.Common
{
    // [EDUCATIVO] Los Middlewares son piezas de código que se ejecutan en CADA petición que entra o sale.
    public class ExceptionMiddleware
    {
        // Este delegado representa "el siguiente paso" en la tubería (pipeline) de la petición.
        private readonly RequestDelegate _next;

        // Si queremos logs, inyectamos ILogger (muy recomendado para producción)
        private readonly ILogger<ExceptionMiddleware> _logger;

        // Constructor: Recibimos el siguiente paso (_next) y el logger.
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        // [EDUCATIVO] Este método 'InvokeAsync' es MÁGICO. ASP.NET lo llama automáticamente en cada petición.
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Le decimos a la petición: "Pasa al siguiente nivel (Controlador, etc.)"
                // await _next(context) es como decir "Intenta ejecutar la lógica normal".
                await _next(context);
            }
            catch (Exception ex)
            {
                // [EDUCATIVO] ¡AJÁ! Si algo explotó en CUALQUIER lugar abajo de nosotros (Controlador, Servicio, Cliente),
                // el error sube burbujeando hasta aquí. Lo atrapamos y lo manejamos bonito.

                _logger.LogError(ex, "Algo salió mal: {Message}", ex.Message); // Logueamos para nosotros (los devs)

                await HandleExceptionAsync(context, ex); // Le respondemos al usuario
            }
        }

        // Aquí construimos la respuesta JSON bonita para el cliente
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Definimos el código y mensaje por defecto (500)
            int statusCode = (int)HttpStatusCode.InternalServerError;
            string message = "Ocurrió un error interno en el servidor.";

            // [EDUCATIVO] Personalizamos la respuesta según el tipo de excepción
            switch (exception)
            {
                case KeyNotFoundException: // Recurso no encontrado
                    statusCode = (int)HttpStatusCode.NotFound;
                    message = "El recurso solicitado no fue encontrado.";
                    break;
                
                case UnauthorizedAccessException: // Sin permiso
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    message = "No tienes permisos para realizar esta acción.";
                    break;
                
                case ArgumentException: // Datos inválidos enviados por el cliente
                    statusCode = (int)HttpStatusCode.BadRequest;
                    message = "Los datos enviados son incorrectos o incompletos.";
                    break;
                
                case InvalidOperationException: // Estado inválido de la aplicación
                    statusCode = (int)HttpStatusCode.Conflict; // O BadRequest, depende del caso
                    message = "No se puede completar la operación en el estado actual.";
                    break;

                case HttpRequestException: // Error al llamar a otro microservicio
                    statusCode = (int)HttpStatusCode.BadGateway;
                    message = "Error de comunicación con un servicio externo.";
                    break;

                // Puedes agregar más casos aquí según tus necesidades
            }

            context.Response.StatusCode = statusCode;

            var response = new
            {
                StatusCode = statusCode,
                Message = message,
                Detailed = exception.Message // Útil para depuración, ocultar en prod si se desea
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}