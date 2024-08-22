using Microsoft.Extensions.Configuration;
using StreamListener.Helpers;
using StreamListener.Twitch;

namespace StreamListener;

public class Program
{
    public static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        
        await WebSocketServerManager.StartServer();

        TiktokConnectionManager.ConnectToTiktok(); // We do not want to wait this because it will hold up the server
        TwitchConnectionManager.ConnectToTwitch(config["TwitchToken"]);
        
        await WebSocketServerManager.ListenForConnections();
    }
}