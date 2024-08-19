namespace StreamListener.Helpers;

public class Logger
{
    public static void Info(string message, ConsoleColor color = ConsoleColor.White)
    {
        SetConsoleColor(color);
        Console.WriteLine(message);
    }

    public static void Warn(string message, ConsoleColor color = ConsoleColor.Yellow)
    {
        SetConsoleColor(color);
        Console.WriteLine(message);
    }

    public static void Error(string message, ConsoleColor color = ConsoleColor.Red)
    {
        SetConsoleColor(color);
        Console.WriteLine(message);
    }

    private static void SetConsoleColor(ConsoleColor color)
    {
        if (Console.ForegroundColor != color)
            Console.ForegroundColor = color;
    }
}