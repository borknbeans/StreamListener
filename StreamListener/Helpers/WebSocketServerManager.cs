﻿using System.Net;
using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;

namespace StreamListener.Helpers;

public class WebSocketServerManager
{
    private static HttpListener _listener;
    
    public static async Task StartServer()
    {
        _listener = new HttpListener();
        _listener.Prefixes.Add("http://localhost:5000/");
        _listener.Start();
        Logger.Info("Server started...", ConsoleColor.Green);
    }

    public static async Task ListenForConnections()
    {
        while (true)
        {
            var context = await _listener.GetContextAsync();

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
        try
        {
            var buffer = new byte[1024 * 4];

            // Wait for the first message from the client
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (result.MessageType != WebSocketMessageType.Close)
            {
                // Decode the received message
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                HandleMessage(webSocket, message);

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            // Close the websocket connection
            await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
        }
        catch (WebSocketException e)
        {
            Logger.Warn("Web Socket disconnected unexpectedly", ConsoleColor.Red);
            await SubscriptionManager.RemoveAllWebSocketsSubscriptions(webSocket);
        }
        
    }

    private static void HandleMessage(WebSocket webSocket, string message)
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
}