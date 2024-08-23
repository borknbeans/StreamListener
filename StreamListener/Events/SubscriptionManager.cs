using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace StreamListener.Helpers;

public class SubscriptionManager
{
    private static readonly Dictionary<Events, List<WebSocket>> _subscriptions = new();

    public static void AddSubscription(Events eventType, WebSocket webSocket)
    {
        if (_subscriptions.TryGetValue(eventType, out var subscription))
        {
            // Existing key
            subscription.Add(webSocket);
        }
        else
        {
            // Key doesn't exist yet
            _subscriptions.Add(eventType, new List<WebSocket> { webSocket });
        }

        Logger.Info($"Added subscription for {eventType}");
    }

    public static List<WebSocket> GetSubscriptions(Events eventType)
    {
        if (_subscriptions.TryGetValue(eventType, out var subscription))
        {
            return subscription;
        }

        return new List<WebSocket>();
    }

    public static async Task NotifySubscriptions<T>(BaseMessage<T> message)
    {
        List<WebSocket> subscriptions = GetSubscriptions(message.Type);
        
        var json = JsonSerializer.Serialize(message);
        var data = Encoding.UTF8.GetBytes(json);

        foreach (var webSocket in subscriptions)
        {
            await webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }
    }

    public static async Task RemoveAllWebSocketsSubscriptions(WebSocket webSocket)
    {
        foreach (var key in _subscriptions.Keys)
        {
            Console.WriteLine($"Removing subscription for {key}");
            _subscriptions[key].Remove(webSocket);
        }
    }
    
    public static async Task NotifySubscriptions(Events eventType, byte[] data)
    {
        List<WebSocket> subscriptions = GetSubscriptions(eventType);

        foreach (var webSocket in subscriptions)
        {
            await webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true,
                CancellationToken.None);
        }
    }
}