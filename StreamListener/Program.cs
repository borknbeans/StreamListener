using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Newtonsoft.Json;
using StreamListener.Helpers;
using StreamListener.Tiktok;
using TikTokLiveSharp.Client;
using JsonException = System.Text.Json.JsonException;

namespace StreamListener;

public class Program
{
    private const string TiktokName = "happyhappygaltv";
    
    public static async Task Main(string[] args)
    {
        var httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://localhost:5000/");
        httpListener.Start();
        Logger.Info("Server started...", ConsoleColor.Green);

        //await ConnectToTiktok();
        
        while (true)
        {
            var context = await httpListener.GetContextAsync();

            if (context.Request.IsWebSocketRequest)
            {
                var wsContext = await context.AcceptWebSocketAsync(null);
                
                await HandleIncomingWebSocketMessages(wsContext.WebSocket);
            }
            else
            {
                // Reject non-WebSocket requests with a 400 Bad Request response
                context.Response.StatusCode = 400;
                context.Response.Close();
            }
        }
    }

    private static async Task HandleIncomingWebSocketMessages(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        
        // Wait for the first message from the client
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (result.MessageType != WebSocketMessageType.Close)
        {
            // Decode the received message
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            await HandleMessage(webSocket, message);
            
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        
        // Close the websocket connection
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
    }

    private static async Task HandleMessage(WebSocket webSocket, string message)
    {
        Logger.Info($"Received: {message}");

        try
        {
            var subscriptionMessage = JsonConvert.DeserializeObject<SubscriptionMessage>(message);

            if (subscriptionMessage == null) { return; }
            
            foreach (var subscriptionEvent in subscriptionMessage.Events)
            {
                var result = Events.TryParse(subscriptionEvent, true, out Events eventType);

                if (result)
                {
                    SubscriptionManager.AddSubscription(eventType, webSocket);
                }
            }
        }
        catch (JsonException e)
        {
            Logger.Error($"Failed to deserialize message: {e.Message}");
        }
    }

    private static async Task ConnectToTiktok()
    {
        try
        {
            TikTokLiveClient client = new TikTokLiveClient(TiktokName, "");
            client.OnConnected += TiktokHandlers.OnConnected;
            client.OnDisconnected += TiktokHandlers.OnDisconnected;
            client.OnRoomUpdate += TiktokHandlers.OnViewerData;
            client.OnLiveEnded += TiktokHandlers.OnLiveEnded;
            client.OnJoin += TiktokHandlers.OnJoin;
            client.OnChatMessage += TiktokHandlers.OnComment;
            client.OnFollow += TiktokHandlers.OnFollow;
            client.OnShare += TiktokHandlers.OnShare;
            client.OnSubscribe += TiktokHandlers.OnSubscribe;
            client.OnLike += TiktokHandlers.OnLike;
            client.OnGiftMessage += TiktokHandlers.OnGiftMessage;
            client.OnEmoteChat += TiktokHandlers.OnEmote;

            await client.RunAsync(new CancellationToken());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}