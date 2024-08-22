namespace StreamListener.Helpers.Payloads;

public class CommandPayload
{
    public string Command { get; set; }
    public IEnumerable<string> Args { get; set; }
}