using Lab1_3.Interfaces;

namespace Lab1_3.UI.Console;

public class ConsoleView : IConsoleView
{
    public void DisplayHeader(string title)
    {
        var oldColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Cyan;
        System.Console.WriteLine($"\n=== {title} ===");
        System.Console.ForegroundColor = oldColor;
    }

    public void DisplayInfo(string message)
    {
        System.Console.WriteLine(message);
    }

    public void DisplayError(string error)
    {
        var oldColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Red;
        System.Console.WriteLine($"Помилка: {error}");
        System.Console.ForegroundColor = oldColor;
    }

    public void DisplaySuccess(string message)
    {
        var oldColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Green;
        System.Console.WriteLine(message);
        System.Console.ForegroundColor = oldColor;
    }

    public void DisplayWarning(string message)
    {
        var oldColor = System.Console.ForegroundColor;
        System.Console.ForegroundColor = ConsoleColor.Yellow;
        System.Console.WriteLine($"Увага: {message}");
        System.Console.ForegroundColor = oldColor;
    }

    public void DisplayLine()
    {
        System.Console.WriteLine(new string('-', 50));
    }

    public void DisplayMenu(string[] options)
    {
        for (var i = 0; i < options.Length; i++) System.Console.WriteLine($"{i + 1}. {options[i]}");
        System.Console.WriteLine();
    }

    public void DisplayMenu(IList<string> options)
    {
        string[] optionsArray = new string[options.Count];
        for (var i = 0; i < options.Count; i++) optionsArray[i] = options[i];
        DisplayMenu(optionsArray);
    }

    public string GetUserInput(string prompt)
    {
        System.Console.Write(prompt);
        return System.Console.ReadLine() ?? string.Empty;
    }

    public string GetInput(string prompt)
    {
        return GetUserInput(prompt);
    }

    public void WaitForKeyPress(string message = "Натисніть будь-яку клавішу для продовження...")
    {
        System.Console.WriteLine(message);
        System.Console.ReadKey(true);
    }

    public void WaitForKeyPress()
    {
        WaitForKeyPress("\nНатисніть будь-яку клавішу для продовження...");
    }

    public void ClearScreen()
    {
        System.Console.Clear();
    }

    public void Clear()
    {
        ClearScreen();
    }
}