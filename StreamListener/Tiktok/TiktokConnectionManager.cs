using StreamListener.Tiktok;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Client.Config;
using TikTokLiveSharp.Errors.Connections;

namespace StreamListener.Helpers;

public class TiktokConnectionManager
{
    private const string TiktokName = "borknbean";
    
    public static async Task ConnectToTiktok()
    {
        try
        {
            ClientSettings settings = new ClientSettings();
            settings.HandleExistingMessagesOnConnect = false;
            
            TikTokLiveClient client = new TikTokLiveClient(TiktokName, "", settings);
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
        catch (LiveNotFoundException e)
        {
            Logger.Warn("Failed to connect to TikTok", ConsoleColor.Red);
        }
    }
}