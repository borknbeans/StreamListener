namespace StreamListener.Helpers;

public class SubscriptionMessage
{
    public string Type { get; set; }
    public List<string> Events { get; set; }
}