using System.Net;
using System.Text.Json;
using SB_Application.DTOs;
using SB_Domain.Exceptions;

namespace SB_VehicleInventoryMicroservice.Middleware
{
    public class SB_GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SB_GlobalExceptionMiddleware> _logger;

        public SB_GlobalExceptionMiddleware(RequestDelegate next, ILogger<SB_GlobalExceptionMiddleware> logger)
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, message) = exception switch
            {
                SB_InvalidVehicleStateException => (HttpStatusCode.BadRequest, exception.Message),
                KeyNotFoundException => (HttpStatusCode.NotFound, exception.Message),
                ArgumentException => (HttpStatusCode.BadRequest, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred.")
            };

            _logger.LogError(exception, "Exception caught by global handler: {Message}", exception.Message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var errorResponse = new SB_ErrorResponse
            {
                StatusCode = (int)statusCode,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(json);
        }
    }
}
