using Lab1_3.Interfaces;
using Lab1_3.Services;

namespace Lab1_3.UI.Console.Menus;

public class MainMenu
{
    private readonly AccountMenu _accountMenu;

    private readonly string[] _menuOptions =
    {
        "Показати доступні пристрої",
        "Вибрати пристрій",
        "Додати новий пристрій",
        "Знайти та встановити гру",
        "Керувати іграми на поточному пристрої",
        "Запустити гру",
        "Керувати запущеною грою",
        "Створити нову гру",
        "Керувати обліковими записами",
        "Вихід"
    };

    private readonly GameSimulator _simulator;
    private readonly IConsoleView _view;

    public MainMenu(IConsoleView view, GameSimulator simulator, AccountMenu accountMenu)
    {
        _view = view;
        _simulator = simulator;
        _accountMenu = accountMenu;
    }

    public void Show()
    {
        var exit = false;

        while (!exit)
        {
            _view.ClearScreen();
            _view.DisplayHeader("Головне меню");

            if (_simulator.CurrentDevice != null)
                _view.DisplayInfo(
                    $"Поточний пристрій: {_simulator.CurrentDevice.Name} ({_simulator.CurrentDevice.GetDeviceType()})");
            else
                _view.DisplayInfo("Поточний пристрій не вибраний");

            _view.DisplayInfo(string.Empty);
            _view.DisplayMenu(_menuOptions);

            var input = _view.GetUserInput("Виберіть опцію: ");

            if (!_simulator.ValidationService.IsValidMenuOption(input, 0, _menuOptions.Length - 1, out var choice))
            {
                _view.DisplayError($"Невірний вибір. Будь ласка, введіть число від 0 до {_menuOptions.Length - 1}: ");
                _view.WaitForKeyPress();
                continue;
            }

            switch (choice)
            {
                case 1:
                    _simulator.ShowDevices();
                    break;
                case 2:
                    _simulator.SelectCurrentDevice();
                    break;
                case 3:
                    _simulator.AddDevice();
                    break;
                case 4:
                    _simulator.FindAndInstallGame();
                    break;
                case 5:
                    _simulator.ManageGamesOnDevice();
                    break;
                case 6:
                    _simulator.RunGame();
                    break;
                case 7:
                    _simulator.ManageRunningGame();
                    break;
                case 8:
                    _simulator.CreateNewGame();
                    break;
                case 9:
                    _accountMenu.Show();
                    break;
                case 0:
                    exit = true;
                    break;
                default:
                    _view.DisplayError("Невірний вибір. Спробуйте ще раз.");
                    _view.WaitForKeyPress();
                    break;
            }
        }
    }
}