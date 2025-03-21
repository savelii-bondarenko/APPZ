using System.Text;
using Lab1_3.Services;

namespace Lab1_3;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("Симулятор");

        var gamesJsonPath = "games.json";

        try
        {
            var simulator = new GameSimulator(gamesJsonPath);
            simulator.Run();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Критична помилка: {ex.Message}");
            Console.ResetColor();
        }
    }
}