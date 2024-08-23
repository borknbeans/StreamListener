using StreamListener.Helpers;
using StreamListener.Helpers.Payloads;
using TwitchLib.Client.Events;

namespace StreamListener.Twitch;

public class TwitchHandlers
{
    public static async Task OnComment(object sender, OnMessageReceivedArgs e)
    {
        Logger.Info($"[Twitch] {e.ChatMessage.Username}: {e.ChatMessage.Message}", ConsoleColor.Yellow);
        
        if (e.ChatMessage.Message.StartsWith("!"))
        {
            CommandHandler.OnCommand(Sources.Twitch, e.ChatMessage.Username, e.ChatMessage.Username, true, e.ChatMessage.Message.Substring(1));
        }
        var message = new CommentMessage
        {
            Identifier = e.ChatMessage.Username,
            Source = Sources.Twitch,
            Follower = true,
            Payload = new CommentPayload
            {
                Message = e.ChatMessage.Message
            }
        };
        
        await SubscriptionManager.NotifySubscriptions(message);
    }
}