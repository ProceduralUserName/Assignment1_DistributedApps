using System.Text.Json;

namespace SB_ApiGateway.Middleware
{
    public class SB_ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HashSet<string> _apiKeys;
        private readonly string _gatewaySecret;

        public SB_ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;

            var apiKeys = configuration.GetSection("Gateway:ApiKeys").Get<string[]>();
            if (apiKeys == null || apiKeys.Length == 0)
                throw new InvalidOperationException("Gateway:ApiKeys configuration is missing or empty.");

            _apiKeys = new HashSet<string>(apiKeys);

            _gatewaySecret = configuration["Gateway:Secret"]
                ?? throw new InvalidOperationException("Gateway:Secret configuration is missing.");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var apiKeyHeader)
                || string.IsNullOrWhiteSpace(apiKeyHeader))
            {
                await WriteUnauthorizedResponse(context, "API key is missing. Provide a valid X-Api-Key header.");
                return;
            }

            string apiKey = apiKeyHeader.ToString();

            if (!_apiKeys.Contains(apiKey))
            {
                await WriteUnauthorizedResponse(context, "Invalid API key.");
                return;
            }

            context.Request.Headers["X-Gateway-Secret"] = _gatewaySecret;

            await _next(context);
        }

        private static async Task WriteUnauthorizedResponse(HttpContext context, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

            var response = new
            {
                StatusCode = StatusCodes.Status401Unauthorized,
                Message = message,
                Timestamp = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
