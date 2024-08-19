using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class GiftMessage : BaseMessage<GiftPayload>
{
    public GiftMessage() : base(Events.OnGiftMessage) { }
}