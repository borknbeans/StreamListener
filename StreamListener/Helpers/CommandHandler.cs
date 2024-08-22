using System.Speech.Synthesis;
using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class CommandHandler
{
    static SpeechSynthesizer synth = null;
    
    public static async void OnCommand(string username, string nickname, bool follower, string command)
    {
        List<string> args = command.ToLower().Split(' ').ToList();
        
        if (args[0].Equals("tts") && follower)
        {
            TextToSpeech(nickname, command.Substring(command.IndexOf(' ') + 1));
        }
        else
        {
            var message = new CommandMessage
            {
                Identifier = username,
                Follower = follower,
                Payload = new CommandPayload
                {
                    Command = args[0],
                    Args = args.Skip(1)
                }
            };
        
            await SubscriptionManager.NotifySubscriptions(message);
        }
    }

    private static void TextToSpeech(string name, string message)
    {
        if (synth == null)
        {
            synth = new SpeechSynthesizer();
            synth.Volume = 75;
            synth.SelectVoiceByHints(VoiceGender.Female);
            synth.SetOutputToDefaultAudioDevice();
        }

        if (message.Length > 100)
        {
            message = message.Substring(0, 100);
        }

        switch (message)
        {
            case "cat":
                synth.SpeakAsync("mooooo");
                break;
            case "cow":
                synth.SpeakAsync("meow");
                break;
            case "bunker":
                synth.SpeakAsync("woof");
                break;
            default:
                synth.SpeakAsync($"{name} says {message}");
                break;
        }
    }
}