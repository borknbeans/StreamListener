using System.Text.Json.Serialization;

namespace StreamListener.Helpers;

public abstract class BaseMessage<TPayload>
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Events Type { get; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Sources Source { get; set; }
    public string Identifier { get; set; }
    public bool Follower { get; set; }
    public TPayload Payload { get; set; }

    protected BaseMessage(Events type)
    {
        Type = type;
    }
}