using StreamListener.Tiktok;
using TikTokLiveSharp.Client;

namespace StreamListener.Helpers;

public class TiktokConnectionManager
{
    private const string TiktokName = "jprospering";
    
    public static async Task ConnectToTiktok()
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