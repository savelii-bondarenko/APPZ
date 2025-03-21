using lab1_appz.Services;

namespace lab1_appz
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            
            Console.WriteLine("Симулятор");
            
            string gamesJsonPath = "games.json";
            
            try
            {
                GameSimulator simulator = new GameSimulator(gamesJsonPath);
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
}
