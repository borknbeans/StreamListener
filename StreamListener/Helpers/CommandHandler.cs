using System.Speech.Synthesis;
using NAudio.Wave;
using StreamListener.Helpers.Payloads;

namespace StreamListener.Helpers;

public class CommandHandler
{
    private static SpeechSynthesizer synth = null;
    private static SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    
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

    private static async void TextToSpeech(string name, string message)
    {
        if (synth == null)
        {
            synth = new SpeechSynthesizer();
            synth.Volume = 75;
            synth.SelectVoiceByHints(VoiceGender.Female);
        }

        if (message.Length > 100)
        {
            message = message.Substring(0, 100);
        }

        await semaphore.WaitAsync();  // Wait for the previous task to finish
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                // Set the output to a memory stream
                synth.SetOutputToWaveStream(memoryStream);

                // Handle the message with the synthesizer
                switch (message)
                {
                    case "cat":
                        synth.Speak("mooooo");
                        break;
                    case "cow":
                        synth.Speak("meow");
                        break;
                    case "bunker":
                        synth.Speak("woof");
                        break;
                    default:
                        synth.Speak($"{name} says {message}");
                        break;
                }

                // Reset the memory stream position for reading
                memoryStream.Position = 0;

                // Play the audio through a specific device asynchronously
                await Task.Run(() =>
                {
                    using (var waveOut = new WaveOutEvent())
                    {
                        waveOut.DeviceNumber = 1;
                        using (var waveProvider = new WaveFileReader(memoryStream))
                        {
                            waveOut.Init(waveProvider);
                            waveOut.Play();
                            while (waveOut.PlaybackState == PlaybackState.Playing)
                            {
                                Thread.Sleep(100);  // Keep the task alive during playback
                            }
                        }
                    }
                });
            }
        }
        finally
        {
            semaphore.Release();  // Release the semaphore when done
        }
    }
}