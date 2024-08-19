using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class FollowerMessage : BaseMessage<FollowerPayload>
{
    public FollowerMessage() : base(Events.OnFollow) { }
}