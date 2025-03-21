namespace Lab1_3.Interfaces;

public interface IConsoleView
{
    void DisplayHeader(string title);
    void DisplayInfo(string message);
    void DisplayError(string error);
    void DisplaySuccess(string message);
    void DisplayWarning(string message);
    void DisplayLine();
    void DisplayMenu(string[] options);
    void DisplayMenu(IList<string> options);
    string GetUserInput(string prompt);
    string GetInput(string prompt);
    void WaitForKeyPress(string message = "Натисніть будь-яку клавішу для продовження...");
    void WaitForKeyPress();
    void ClearScreen();
    void Clear();
}