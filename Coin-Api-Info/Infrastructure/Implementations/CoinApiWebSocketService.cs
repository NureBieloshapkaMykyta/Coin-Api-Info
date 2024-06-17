using Application.Abstractions;
using Microsoft.Extensions.Configuration;
using System.Net.WebSockets;
using System.Text;

namespace Infrastructure.Implementations;

public class CoinApiWebSocketService : ICoinApiWebSocketService
{
    private readonly ClientWebSocket _webSocket;

    private const string _webSocketUri = "wss://ws.coinapi.io/v1/";

    private readonly string apiKey;

    public CoinApiWebSocketService(IConfiguration configuration)
    {
        _webSocket = new ClientWebSocket();
        apiKey = configuration.GetSection("CoinApi").GetValue<string>("ApiKey");
    }

    public async Task ConnectAsync()
    {
        try
        {
            _webSocket.Options.SetRequestHeader("X-CoinAPI-Key", apiKey);
            await _webSocket.ConnectAsync(new Uri(_webSocketUri), CancellationToken.None);

            var subscribeMessage = Encoding.UTF8.GetBytes("{\"type\": \"hello\", \"apikey\": \"" + apiKey + "\", \"heartbeat\": false, \"subscribe_data_type\": [\"trade\", \"quote\"]}");
            await _webSocket.SendAsync(new ArraySegment<byte>(subscribeMessage), WebSocketMessageType.Text, true, CancellationToken.None);

            var buffer = new byte[1024 * 4];
            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received: " + message);
            }
        }
        finally
        {
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
    }
}
