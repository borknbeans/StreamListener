using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class LikeMessage : BaseMessage<LikePayload>
{
    public LikeMessage() : base(Events.OnLike) { }
}