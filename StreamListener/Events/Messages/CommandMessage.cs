using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class CommandMessage : BaseMessage<CommandPayload>
{
    public CommandMessage() : base(Events.OnCommand) { }
}