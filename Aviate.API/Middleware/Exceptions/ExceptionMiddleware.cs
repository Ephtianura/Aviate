using System.Net;
using Aviate.Application.Exceptions;
using FluentValidation;

namespace Aviate.API.Middleware.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, _logger);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            HttpStatusCode status;
            string message;            

            // Обробка для FluentValidation
            if (exception is ValidationException validationEx)
            {
                status = HttpStatusCode.BadRequest;
                message = string.Join("; ", validationEx.Errors.Select(e => e.ErrorMessage));
            }
            else
            {
                // Обираємо статус по помилці
                status = ExceptionGroups.Groups
                    .FirstOrDefault(g => g.Value.Contains(exception.GetType()))
                    .Key;

                // За замовчуванням - InternalServerError
                if (status == 0)
                {
                    status = HttpStatusCode.InternalServerError;
                    logger.LogError(exception, "Unhandled exception");
                }

                message = exception.Message;
            }

            context.Response.StatusCode = (int)status;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}
