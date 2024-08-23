using StreamListener.Helpers;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;

namespace StreamListener.Twitch;

public class TwitchConnectionManager
{
    public static async Task ConnectToTwitch(string accessToken)
    {
        if (accessToken == null)
        {
            Logger.Warn("No access token provided", ConsoleColor.Red);
            return;
        }
        
        try
        {
            ConnectionCredentials credentials = new ConnectionCredentials("borknbean", accessToken);
            WebSocketClient webSocket = new WebSocketClient();
            
            TwitchClient client = new TwitchClient(webSocket);
            client.Initialize(credentials, "borknbean");

            client.OnMessageReceived += TwitchHandlers.OnComment;
            
            client.Connect();
            
            Logger.Info("Connected to Twitch", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Logger.Warn("Failed to connect to Twitch", ConsoleColor.Red);
        }
    }
}