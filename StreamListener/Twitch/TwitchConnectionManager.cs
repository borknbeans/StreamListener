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
            Logger.Warn("No access token provided");
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
            
            Logger.Info("Connected to Twitch");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}