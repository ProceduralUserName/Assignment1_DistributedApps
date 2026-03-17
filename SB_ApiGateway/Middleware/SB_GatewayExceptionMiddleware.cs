using System.Text.Json;

namespace SB_ApiGateway.Middleware
{
    public class SB_GatewayExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SB_GatewayExceptionMiddleware> _logger;

        public SB_GatewayExceptionMiddleware(RequestDelegate next, ILogger<SB_GatewayExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception in API Gateway: {Message}", ex.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status502BadGateway;

                var response = new
                {
                    StatusCode = StatusCodes.Status502BadGateway,
                    Message = "An error occurred while processing your request through the gateway.",
                    Timestamp = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
