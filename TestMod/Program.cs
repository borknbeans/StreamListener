
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace StreamListener;

public class Program
{
    public static async Task Main(string[] args)
    {
        var webSocket = new ClientWebSocket();
        await webSocket.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);
        Console.WriteLine("Connected to server...");
        
        var subscriptionData = new SubscriptionMessage
        {
            Events = new List<string> { "OnComment", "OnFollow" }
        };
        
        // Serialize the subscription data to JSON
        var message = JsonSerializer.Serialize(subscriptionData);
        var bytes = Encoding.UTF8.GetBytes(message);
        
        // Send the JSON message to the server
        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine("Subscription message sent to the server.");
        
        await HandleIncomingMessages(webSocket);
    }

    private static async Task HandleIncomingMessages(ClientWebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        
        // Wait for the first message from the client
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (result.MessageType != WebSocketMessageType.Close)
        {
            // Decode the received message
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received: {message}");
            
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        
        // Close the websocket connection
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);

    }
    
    private class SubscriptionMessage
    {
        public String Type = "subscribe";
        public List<string> Events { get; set; }
    }
}