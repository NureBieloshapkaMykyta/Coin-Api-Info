using Application.Abstractions;
using Infrastructure.Implementations;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace Coin_Api_Info.Middlewares;

public class WebSocketMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<WebSocketMiddleware> _logger;
    private readonly IServiceProvider _serviceProvider;

    public WebSocketMiddleware(RequestDelegate next, ILogger<WebSocketMiddleware> logger, IServiceProvider serviceProvider)
    {
        _next = next;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var socketId = Guid.NewGuid().ToString();
            _logger.LogInformation($"WebSocket connection established: {socketId}");

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _logger.LogInformation($"Received message: {message}");

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var coinApiService = scope.ServiceProvider.GetRequiredService<ICoinApiWebSocketService>();

                        await coinApiService.ConnectAsync();
                    }
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.LogInformation($"WebSocket connection closed: {socketId}");
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
                }
            });
        }
        else
        {
            await _next(context);
        }
    }

    private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
        var buffer = new byte[1024 * 4];
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            handleMessage(result, buffer);
        }
    }
}
