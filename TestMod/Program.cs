
using System.Net.WebSockets;
using System.Speech.Synthesis;
using System.Text;
using System.Text.Json;
using NAudio.Wave;

namespace StreamListener;

public class Program
{
    private static SpeechSynthesizer synth;
    
    public static async Task Main(string[] args)
    {
        /*
        var webSocket = new ClientWebSocket();
        await webSocket.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);
        Console.WriteLine("Connected to server...");
        
        var subscriptionData = new SubscriptionMessage
        {
            Events = new List<string> { "OnComment", "OnFollow" }
        };
        
        // Serialize the subscription data to JSON
        var message = JsonSerializer.Serialize(subscriptionData);
        var bytes = Encoding.UTF8.GetBytes(message);
        
        // Send the JSON message to the server
        await webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
        Console.WriteLine("Subscription message sent to the server.");
        
        await HandleIncomingMessages(webSocket);
        */

        while (true)
        {
            string result = Console.ReadLine();
            
            List<string> arguments = result.ToLower().Split(' ').ToList();
        
            if (arguments[0].Contains("!tts"))
            {
                TextToSpeech(result.Substring(0, result.IndexOf(':') - 1), result.Substring(result.IndexOf(' ') + 1));
            }
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

            // Play the audio through a specific device using NAudio
            using (var waveOut = new WaveOutEvent())
            {
                waveOut.DeviceNumber = 1;
                using (var waveProvider = new WaveFileReader(memoryStream))
                {
                    waveOut.Init(waveProvider);
                    waveOut.Play();
                    while (waveOut.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(100);
                    }
                }
            }
        }
    }

    /*
    private static async Task HandleIncomingMessages(ClientWebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        
        // Wait for the first message from the client
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        while (result.MessageType != WebSocketMessageType.Close)
        {
            // Decode the received message
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Console.WriteLine($"Received: {message}");
            
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        
        // Close the websocket connection
        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by client", CancellationToken.None);

    }
    
    private class SubscriptionMessage
    {
        public String Type = "subscribe";
        public List<string> Events { get; set; }
    }
    */
}