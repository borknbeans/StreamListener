using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class CommentMessage : BaseMessage<CommentPayload>
{
    public CommentMessage() : base(Events.OnComment) { }
}