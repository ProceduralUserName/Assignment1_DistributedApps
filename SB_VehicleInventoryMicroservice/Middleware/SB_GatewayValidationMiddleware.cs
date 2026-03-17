using System.Text.Json;

namespace SB_VehicleInventoryMicroservice.Middleware
{
    public class SB_GatewayValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _expectedSecret;

        public SB_GatewayValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            _expectedSecret = configuration["Gateway:Secret"]
                ?? throw new InvalidOperationException("Gateway:Secret configuration is missing.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;

            if (path.StartsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("X-Gateway-Secret", out var secretHeader)
                || secretHeader.ToString() != _expectedSecret)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status403Forbidden;

                var response = new
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Message = "Direct access is forbidden. Use the API Gateway.",
                    Timestamp = DateTime.UtcNow
                };

                var json = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(json);
                return;
            }

            await _next(context);
        }
    }
}
