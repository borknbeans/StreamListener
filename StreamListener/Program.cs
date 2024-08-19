using StreamListener.Helpers;

namespace StreamListener;

public class Program
{
    public static async Task Main(string[] args)
    {
        await WebSocketServerManager.StartServer();

        TiktokConnectionManager.ConnectToTiktok(); // We do not want to wait this because it will hold up the server

        await WebSocketServerManager.ListenForConnections();
    }
}