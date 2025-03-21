using Lab1_3.enums;
using Lab1_3.Interfaces;
using Lab1_3.Models;
using Lab1_3.Services;

namespace Lab1_3.UI.Console.Menus;

public class GameMenu
{
    private readonly EventSystem _eventSystem;
    private readonly GameRepository _gameRepository;
    private readonly ValidationService _validationService;
    private readonly IConsoleView _view;

    public GameMenu(IConsoleView view, ValidationService validationService, GameRepository gameRepository,
        EventSystem eventSystem)
    {
        _view = view;
        _validationService = validationService;
        _gameRepository = gameRepository;
        _eventSystem = eventSystem;
    }

    public bool FindAndInstallGame(Device device)
    {
        _view.ClearScreen();
        _view.DisplayHeader("Пошук та встановлення гри");

        string[] searchOptions =
        {
            "Показати всі доступні ігри",
            "Пошук за назвою",
            "Пошук за жанром",
            "Показати онлайн-ігри"
        };

        _view.DisplayMenu(searchOptions);

        var input = _view.GetUserInput("Виберіть спосіб пошуку (0 для повернення): ");

        if (!_validationService.TryParseIntInRange(input, 0, searchOptions.Length, out var option))
        {
            _view.DisplayError("Невірний вибір.");
            _view.WaitForKeyPress();
            return false;
        }

        if (option == 0) return false;

        List<Game> games = new List<Game>();

        switch (option)
        {
            case 1:
                games = _gameRepository.GetAllGames();
                break;

            case 2:
                var gameName = _view.GetUserInput("Введіть назву гри (або частину назви): ");
                games = _gameRepository.GetAllGames().FindAll(g => g.Name.ToLower().Contains(gameName.ToLower()));
                break;

            case 3:
                string[] genres = Enum.GetNames(typeof(GameGenre));
                _view.DisplayInfo("Доступні жанри:");

                for (var i = 0; i < genres.Length; i++) _view.DisplayInfo($"{i + 1}. {genres[i]}");

                var genreInput = _view.GetUserInput("Виберіть жанр (номер): ");

                if (!_validationService.TryParseIntInRange(genreInput, 1, genres.Length, out var genreIndex))
                {
                    _view.DisplayError("Невірний жанр.");
                    _view.WaitForKeyPress();
                    return false;
                }

                games = _gameRepository.GetGamesByGenre((GameGenre)(genreIndex - 1));
                break;

            case 4:
                games = _gameRepository.GetOnlineGames();
                break;
        }

        _view.DisplayInfo("Знайдені ігри:");

        if (games.Count == 0)
        {
            _view.DisplayInfo("Ігор не знайдено.");
            _view.WaitForKeyPress();
            return false;
        }

        for (var i = 0; i < games.Count; i++) _view.DisplayInfo($"{i + 1}. {games[i]}");

        var gameInput = _view.GetUserInput("Виберіть гру для встановлення (0 для повернення): ");

        if (!_validationService.TryParseIntInRange(gameInput, 0, games.Count, out var gameIndex))
        {
            _view.DisplayError("Невірний вибір.");
            _view.WaitForKeyPress();
            return false;
        }

        if (gameIndex == 0) return false;

        var selectedGame = games[gameIndex - 1];

        if (device.InstallGame(selectedGame))
        {
            _eventSystem.NotifyGameInstalled(this, selectedGame, device);
            _view.DisplaySuccess($"Гру '{selectedGame.Name}' успішно встановлено на пристрій '{device.Name}'");
            _view.WaitForKeyPress();
            return true;
        }

        _view.DisplayError(
            $"Не вдалося встановити гру '{selectedGame.Name}' на пристрій '{device.Name}'. Перевірте вимоги та наявність вільного місця.");
        _view.WaitForKeyPress();
        return false;
    }

    public void ShowInstalledGames(Device device)
    {
        _view.ClearScreen();
        _view.DisplayHeader($"Встановлені ігри на пристрої {device.Name}");

        var installedGames = device.GetInstalledGames();

        if (installedGames.Count == 0)
        {
            _view.DisplayInfo("На цьому пристрої немає встановлених ігор.");
            _view.WaitForKeyPress();
            return;
        }

        for (var i = 0; i < installedGames.Count; i++)
        {
            var runningStatus = installedGames[i] == device.CurrentlyRunningGame ? " [ЗАПУЩЕНО]" : "";
            _view.DisplayInfo($"{i + 1}. {installedGames[i]}{runningStatus}");
        }

        _view.WaitForKeyPress();
    }

    public void ManageGamesOnDevice(Device device)
    {
        var exit = false;

        while (!exit)
        {
            _view.ClearScreen();
            _view.DisplayHeader($"Керування іграми на пристрої {device.Name}");

            var installedGames = device.GetInstalledGames();

            if (installedGames.Count == 0)
            {
                _view.DisplayInfo("На цьому пристрої немає встановлених ігор.");
                _view.WaitForKeyPress();
                return;
            }

            for (var i = 0; i < installedGames.Count; i++)
            {
                var runningStatus = installedGames[i] == device.CurrentlyRunningGame ? " [ЗАПУЩЕНО]" : "";
                _view.DisplayInfo($"{i + 1}. {installedGames[i].Name}{runningStatus}");
            }

            string[] options =
            {
                "Видалити гру",
                "Запустити гру",
                "Повернутися"
            };

            _view.DisplayInfo(string.Empty);
            _view.DisplayMenu(options);

            var input = _view.GetUserInput("Виберіть опцію: ");

            if (!_validationService.TryParseIntInRange(input, 0, options.Length, out var option))
            {
                _view.DisplayError("Невірний вибір.");
                _view.WaitForKeyPress();
                continue;
            }

            switch (option)
            {
                case 0:
                case 3:
                    exit = true;
                    break;

                case 1:
                    UninstallGame(device);
                    break;

                case 2:
                    RunGame(device);
                    break;
            }
        }
    }

    public bool UninstallGame(Device device)
    {
        _view.ClearScreen();
        _view.DisplayHeader($"Видалення гри з пристрою {device.Name}");

        var installedGames = device.GetInstalledGames();

        if (installedGames.Count == 0)
        {
            _view.DisplayInfo("На цьому пристрої немає встановлених ігор.");
            _view.WaitForKeyPress();
            return false;
        }

        for (var i = 0; i < installedGames.Count; i++) _view.DisplayInfo($"{i + 1}. {installedGames[i].Name}");

        var input = _view.GetUserInput("Виберіть гру для видалення (0 для скасування): ");

        if (!_validationService.TryParseIntInRange(input, 0, installedGames.Count, out var gameIndex))
        {
            _view.DisplayError("Невірний вибір.");
            _view.WaitForKeyPress();
            return false;
        }

        if (gameIndex == 0) return false;

        var selectedGame = installedGames[gameIndex - 1];

        if (device.UninstallGame(selectedGame))
        {
            _eventSystem.NotifyGameUninstalled(this, selectedGame, device);
            _view.DisplaySuccess($"Гру '{selectedGame.Name}' успішно видалено з пристрою '{device.Name}'");
            _view.WaitForKeyPress();
            return true;
        }

        _view.DisplayError($"Не вдалося видалити гру '{selectedGame.Name}' з пристрою '{device.Name}'.");
        _view.WaitForKeyPress();
        return false;
    }

    public bool RunGame(Device device)
    {
        _view.ClearScreen();
        _view.DisplayHeader($"Запуск гри на пристрої {device.Name}");

        if (device.CurrentlyRunningGame != null)
        {
            _view.DisplayError($"На пристрої вже запущена гра: {device.CurrentlyRunningGame.Name}");
            _view.DisplayInfo("Зупиніть поточну гру перед запуском нової.");
            _view.WaitForKeyPress();
            return false;
        }

        var installedGames = device.GetInstalledGames();

        if (installedGames.Count == 0)
        {
            _view.DisplayInfo("На цьому пристрої немає встановлених ігор.");
            _view.WaitForKeyPress();
            return false;
        }

        for (var i = 0; i < installedGames.Count; i++)
        {
            _view.DisplayInfo($"{i + 1}. {installedGames[i].Name} ({installedGames[i].Genre})");

            if (installedGames[i].IsOnline) _view.DisplayInfo("   * Потрібен обліковий запис (онлайн-гра)");
        }

        var input = _view.GetUserInput("Виберіть гру для запуску (0 для скасування): ");

        if (!_validationService.TryParseIntInRange(input, 0, installedGames.Count, out var gameIndex))
        {
            _view.DisplayError("Невірний вибір.");
            _view.WaitForKeyPress();
            return false;
        }

        if (gameIndex == 0) return false;

        var selectedGame = installedGames[gameIndex - 1];

        var hasAccount = false;

        if (selectedGame.IsOnline)
        {
            var accountInput = _view.GetUserInput("У вас є обліковий запис для цієї гри? (Так/Ні): ");

            if (!_validationService.TryParseBoolean(accountInput, out hasAccount))
            {
                _view.DisplayError("Невірний ввід. Будь ласка, введіть 'Так' або 'Ні'.");
                _view.WaitForKeyPress();
                return false;
            }

            if (!hasAccount)
            {
                _view.DisplayError("Для запуску онлайн-ігор потрібен обліковий запис!");
                _view.WaitForKeyPress();
                return false;
            }
        }

        if (device.RunGame(selectedGame, hasAccount))
        {
            _eventSystem.NotifyGameStarted(this, selectedGame, device);
            _view.DisplaySuccess($"Гру '{selectedGame.Name}' успішно запущено на пристрої '{device.Name}'");
            _view.WaitForKeyPress();
            return true;
        }

        _view.DisplayError($"Не вдалося запустити гру '{selectedGame.Name}' на пристрої '{device.Name}'.");
        _view.WaitForKeyPress();
        return false;
    }

    public void ManageRunningGame(Device device)
    {
        if (device == null || device.CurrentlyRunningGame == null)
        {
            _view.DisplayError("На пристрої немає запущених ігор.");
            _view.WaitForKeyPress();
            return;
        }

        var exit = false;

        while (!exit)
        {
            _view.ClearScreen();
            _view.DisplayHeader($"Керування грою {device.CurrentlyRunningGame.Name}");

            _view.DisplayInfo($"Пристрій: {device.Name}");
            _view.DisplayInfo(
                $"Запущена гра: {device.CurrentlyRunningGame.Name} ({device.CurrentlyRunningGame.Genre})");

            string[] options =
            {
                "Зупинити гру",
                "Зберегти стан гри",
                "Завантажити збережений стан",
                "Встановити рейтинг гри",
                "Транслювати гру",
                "Повернутися"
            };

            _view.DisplayInfo(string.Empty);
            _view.DisplayMenu(options);

            var input = _view.GetUserInput("Виберіть опцію");

            if (!_validationService.IsValidMenuOption(input, 1, options.Length, out var option))
            {
                _view.DisplayError("Невірний вибір. Будь ласка, введіть число від 1 до " + options.Length + ": ");
                _view.WaitForKeyPress();
                continue;
            }

            switch (option)
            {
                case 6:
                    exit = true;
                    break;

                case 1:
                    var gameToStop = device.CurrentlyRunningGame;
                    if (device.StopGame() && gameToStop != null)
                    {
                        _eventSystem.NotifyGameStopped(this, gameToStop, device);
                        _view.DisplaySuccess("Гру успішно зупинено.");
                        _view.WaitForKeyPress();
                    }
                    else
                    {
                        _view.DisplayError("Не вдалося зупинити гру.");
                        _view.WaitForKeyPress();
                    }

                    break;

                case 2:
                    SaveGameState(device);
                    break;

                case 3:
                    LoadGameState(device);
                    break;

                case 4:
                    RateGame(device);
                    break;

                case 5:
                    GoLive(device);
                    break;
            }
        }
    }

    private void GoLive(Device device)
    {
        _view.DisplayInfo("Гру транслюється на живу трансляцію.");
        _view.WaitForKeyPress();
    }

    private void SaveGameState(Device device)
    {
        if (device.CurrentlyRunningGame == null)
        {
            _view.DisplayError("На пристрої немає запущених ігор.");
            _view.WaitForKeyPress();
            return;
        }

        var state = _view.GetUserInput("Введіть опис стану гри для збереження: ");

        if (string.IsNullOrWhiteSpace(state))
        {
            _view.DisplayError("Опис стану не може бути порожнім.");
            _view.WaitForKeyPress();
            return;
        }

        if (device.SaveGame(state))
        {
            _eventSystem.NotifyGameSaved(this, device.CurrentlyRunningGame);
            _view.DisplaySuccess("Стан гри успішно збережено.");
            _view.WaitForKeyPress();
        }
        else
        {
            _view.DisplayError("Не вдалося зберегти стан гри.");
            _view.WaitForKeyPress();
        }
    }

    private void LoadGameState(Device device)
    {
        if (device.CurrentlyRunningGame == null)
        {
            _view.DisplayError("На пристрої немає запущених ігор.");
            _view.WaitForKeyPress();
            return;
        }

        var savedState = device.LoadGame();

        if (savedState != null)
        {
            _eventSystem.NotifyGameLoaded(this, device.CurrentlyRunningGame);
            _view.DisplaySuccess($"Збережений стан успішно завантажено: {savedState}");
            _view.WaitForKeyPress();
        }
        else
        {
            _view.DisplayError("Немає збереженого стану або не вдалося завантажити стан гри.");
            _view.WaitForKeyPress();
        }
    }

    private void RateGame(Device device)
    {
        if (device.CurrentlyRunningGame == null)
        {
            _view.DisplayError("На пристрої немає запущених ігор.");
            _view.WaitForKeyPress();
            return;
        }

        _view.DisplayInfo("Встановіть рейтинг гри від 1 до 5:");
        var input = _view.GetUserInput("Рейтинг");

        if (!_validationService.TryParseIntInRange(input, 1, 5, out var rating))
        {
            _view.DisplayError("Невірний рейтинг. Введіть число від 1 до 5.");
            _view.WaitForKeyPress();
            return;
        }

        if (device.CurrentlyRunningGame.SetRating(rating))
        {
            _view.DisplaySuccess($"Рейтинг гри '{device.CurrentlyRunningGame.Name}' встановлено на {rating}.");
            _view.WaitForKeyPress();
        }
        else
        {
            _view.DisplayError("Не вдалося встановити рейтинг гри.");
            _view.WaitForKeyPress();
        }
    }

    public Game? CreateNewGame()
    {
        _view.ClearScreen();
        _view.DisplayHeader("Створення нової гри");

        var name = _view.GetUserInput("Введіть назву гри: ");
        if (!_validationService.IsValidName(name))
        {
            _view.DisplayError(ValidationService.ValidationErrors["name_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        if (_gameRepository.GetGameByName(name) != null)
        {
            _view.DisplayError("Гра з такою назвою вже існує.");
            _view.WaitForKeyPress();
            return null;
        }

        string[] genres = Enum.GetNames(typeof(GameGenre));
        _view.DisplayInfo("Доступні жанри:");

        for (var i = 0; i < genres.Length; i++) _view.DisplayInfo($"{i + 1}. {genres[i]}");

        var genreInput = _view.GetUserInput("Виберіть жанр (номер): ");

        if (!_validationService.TryParseIntInRange(genreInput, 1, genres.Length, out var genreIndex))
        {
            _view.DisplayError("Невірний жанр.");
            _view.WaitForKeyPress();
            return null;
        }

        var genre = (GameGenre)(genreIndex - 1);

        double processorGHz = 0;
        int ramGB = 0, videoRamGB = 0, hddGB = 0;

        var processorInput = _view.GetUserInput("Введіть мінімальну частоту процесора (GHz, від 1.0 до 5.0): ");
        if (!_validationService.TryParseDoubleInRange(processorInput, 1.0, 5.0, out processorGHz))
        {
            _view.DisplayError(ValidationService.ValidationErrors["decimal_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        var ramInput = _view.GetUserInput("Введіть мінімальний об'єм оперативної пам'яті (GB, від 1 до 64): ");
        if (!_validationService.TryParseIntInRange(ramInput, 1, 64, out ramGB))
        {
            _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        var videoRamInput = _view.GetUserInput("Введіть мінімальний об'єм відеопам'яті (GB, від 0 до 16): ");
        if (!_validationService.TryParseIntInRange(videoRamInput, 0, 16, out videoRamGB))
        {
            _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        var hddInput = _view.GetUserInput("Введіть мінімальний об'єм жорсткого диска (GB, від 1 до 500): ");
        if (!_validationService.TryParseIntInRange(hddInput, 1, 500, out hddGB))
        {
            _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        var isOnline = false;
        var onlineInput = _view.GetUserInput("Чи є гра онлайн? (Так/Ні): ");
        if (!_validationService.TryParseBoolean(onlineInput, out isOnline))
        {
            _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
            _view.WaitForKeyPress();
            return null;
        }

        var requirements = new GameRequirements(processorGHz, ramGB, videoRamGB, hddGB);
        var newGame = new Game(name, genre, requirements, isOnline);

        if (_gameRepository.AddGame(newGame))
        {
            _view.DisplaySuccess($"Гру '{newGame.Name}' успішно створено та додано до репозиторію.");
            _view.WaitForKeyPress();
            return newGame;
        }

        _view.DisplayError("Не вдалося додати гру до репозиторію.");
        _view.WaitForKeyPress();
        return null;
    }
}