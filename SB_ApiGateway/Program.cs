using SB_ApiGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseMiddleware<SB_GatewayExceptionMiddleware>();
app.UseMiddleware<SB_ApiKeyMiddleware>();

app.MapReverseProxy();

app.Run();
