using StreamListener.Helpers;
using TikTokLiveSharp.Client;
using TikTokLiveSharp.Events;

namespace StreamListener.Tiktok;

public class TiktokHandlers
{
    public static void OnConnected(TikTokLiveClient sender, bool e)
    {
        Logger.Info($"Connected to Room! [Connected:{e}]");
    }

    public static void OnDisconnected(TikTokLiveClient sender, bool e)
    {
        Logger.Info($"Disconnected from Room! [Connected:{e}]");
    }

    public static void OnViewerData(TikTokLiveClient sender, RoomUpdate e)
    {
        Logger.Info($"Viewer count is: {e.NumberOfViewers}", ConsoleColor.Cyan);
    }

    public static void OnLiveEnded(TikTokLiveClient sender, ControlMessage e)
    {
        Logger.Info("Host ended Stream!");
    }

    public static void OnJoin(TikTokLiveClient sender, Join e)
    {
        Logger.Info($"{e.User.UniqueId} joined!", ConsoleColor.Green);
    }

    public static void OnComment(TikTokLiveClient sender, Chat e)
    {
        Logger.Info($"{e.Sender.UniqueId}: {e.Message}", ConsoleColor.Yellow);
    }

    public static void OnFollow(TikTokLiveClient sender, Follow e)
    {
        Logger.Info($"{e.User?.UniqueId} followed!", ConsoleColor.DarkRed);
    }

    public static void OnShare(TikTokLiveClient sender, Share e)
    {
        Logger.Info($"{e.User?.UniqueId} shared!", ConsoleColor.Blue);
    }

    public static void OnSubscribe(TikTokLiveClient sender, Subscribe e)
    {
        Logger.Info($"{e.User.UniqueId} subscribed!", ConsoleColor.DarkCyan);
    }

    public static void OnLike(TikTokLiveClient sender, Like e)
    {
        Logger.Info($"{e.Sender.UniqueId} liked!", ConsoleColor.Red);
    }

    public static void OnGiftMessage(TikTokLiveClient sender, GiftMessage e)
    {
        Logger.Info($"{e.User.UniqueId} sent {e.Amount}x {e.Gift.Name}!", ConsoleColor.Magenta);
    }

    public static void OnEmote(TikTokLiveClient sender, EmoteChat e)
    {
        Logger.Info($"{e.User.UniqueId} sent {e.Emotes?.First()?.Id}!", ConsoleColor.DarkGreen);
    }
}