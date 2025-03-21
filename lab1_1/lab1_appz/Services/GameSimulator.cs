using lab1_appz.Models;
using lab1_appz.UI.Console;
using lab1_appz.UI.Console.Menus;

namespace lab1_appz.Services
{
    public class GameSimulator
    {
        private readonly GameRepository _gameRepository;
        private readonly EventSystem _eventSystem;
        private readonly List<Device> _devices = new();
        private readonly IConsoleView _view;
        private readonly ValidationService _validationService;
        private readonly DeviceMenu  _deviceMenu;
        private readonly GameMenu _gameMenu;
        private readonly AccountService _accountService;
        private readonly AccountMenu _accountMenu;
        private readonly MainMenu _mainMenu;
        
        public Device? CurrentDevice { get; private set; }
        public ValidationService ValidationService => _validationService;

        public GameSimulator(string gamesJsonPath)
        {
            _eventSystem = EventSystem.Instance;
            _view = new ConsoleView();
            _validationService = new ValidationService();
            _gameRepository = new GameRepository(gamesJsonPath, _eventSystem);
            _accountService = new AccountService(_eventSystem);
            _deviceMenu = new DeviceMenu(_view, _validationService, _devices, _eventSystem);
            _gameMenu = new GameMenu(_view, _validationService, _gameRepository, _eventSystem);
            _accountMenu = new AccountMenu(_view, _validationService, _accountService, _eventSystem);
            _mainMenu = new MainMenu(_view, this, _accountMenu);
            
            CreateDefaultDevices();
            
            _ = new ConsoleLogger(_eventSystem);
        }

        public Account? CurrentAccount => _accountService.CurrentAccount;

        public void Run()
        {
            _mainMenu.Show();
        }

        private void CreateDefaultDevices()
        {
            var desktop = new DesktopComputer(
                "Gaming PC",
                3.8,
                16,
                6,
                512,
                true,
                true
            );
            _devices.Add(desktop);
            _eventSystem.NotifyDeviceAdded(this, desktop);

            var laptop = new Laptop(
                "Lenovo Laptop",
                2.6,
                8,
                2,
                256,
                6.5,
                false,
                true
            );
            _devices.Add(laptop);
            _eventSystem.NotifyDeviceAdded(this, laptop);

            var console = new GameConsole(
                "PlayStation 5",
                3.5,
                16,
                8,
                825,
                true,
                true,
                true
            );
            _devices.Add(console);
            _eventSystem.NotifyDeviceAdded(this, console);

            var mobile = new MobilePhone(
                "iPhone 15",
                2.4,
                6,
                1,
                128,
                "iOS",
                true,
                6.1
            );
            _devices.Add(mobile);
            _eventSystem.NotifyDeviceAdded(this, mobile);
        }


        public void AddDevice()
        {
            var device = _deviceMenu.AddDevice();
            if (device != null && CurrentDevice == null)
            {
                CurrentDevice = device;
            }
        }

        public void ShowDevices()
        {
            _deviceMenu.ShowDevices();
        }

        public void SelectCurrentDevice()
        {
            var device = _deviceMenu.SelectDevice();
            if (device != null)
            {
                CurrentDevice = device;
            }
        }

        public void FindAndInstallGame()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            _gameMenu.FindAndInstallGame(CurrentDevice);
        }

        public void RunGame()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            if (CurrentDevice.CurrentlyRunningGame == null)
            {
                var installedGames = CurrentDevice.GetInstalledGames();
                if (installedGames.Count > 0)
                {
                    _view.DisplayHeader("Запуск гри");
                    
                    for (int i = 0; i < installedGames.Count; i++)
                    {
                        _view.DisplayInfo($"{i + 1}. {installedGames[i]}");
                    }
                    
                    int gameIndex = _validationService.ParseInt(
                        _view.GetInput("Виберіть гру для запуску (номер) або 0 для скасування: "), 
                        0, 
                        installedGames.Count, 
                        defaultValue: 0);
                    
                    if (gameIndex == 0)
                    {
                        return;
                    }
                    
                    Game selectedGame = installedGames[gameIndex - 1];
                    
                    if (selectedGame.IsOnline)
                    {
                        if (CurrentAccount == null)
                        {
                            _view.DisplayWarning("Для запуску онлайн-гри потрібен обліковий запис!");
                            bool createAccount = _validationService.ParseBool(
                                _view.GetInput("Бажаєте створити або увійти в обліковий запис? (так/ні): "));
                            
                            if (createAccount)
                            {
                                _accountMenu.Show();
                                if (CurrentAccount == null)
                                {
                                    _view.DisplayError("Ви не увійшли в обліковий запис. Запуск гри скасовано.");
                                    _view.WaitForKeyPress();
                                    return;
                                }
                            }
                            else
                            {
                                _view.DisplayInfo("Запуск гри скасовано.");
                                _view.WaitForKeyPress();
                                return;
                            }
                        }
                        
                        _view.DisplayInfo($"Запуск онлайн-гри {selectedGame.Name} з обліковим записом {CurrentAccount!.Username}");
                        
                        if (CurrentDevice.RunGame(selectedGame, true))
                        {
                            _eventSystem.NotifyGameStarted(this, selectedGame, CurrentDevice);
                            _view.DisplaySuccess($"Гра {selectedGame.Name} запущена на пристрої {CurrentDevice.Name}");
                        }
                        else
                        {
                            _view.DisplayError($"Не вдалося запустити гру {selectedGame.Name} на пристрої {CurrentDevice.Name}");
                        }
                    }
                    else
                    {
                        if (CurrentDevice.RunGame(selectedGame, false))
                        {
                            _eventSystem.NotifyGameStarted(this, selectedGame, CurrentDevice);
                            _view.DisplaySuccess($"Гра {selectedGame.Name} запущена на пристрої {CurrentDevice.Name}");
                        }
                        else
                        {
                            _view.DisplayError($"Не вдалося запустити гру {selectedGame.Name} на пристрої {CurrentDevice.Name}");
                        }
                    }
                    
                    _view.WaitForKeyPress();
                }
                else
                {
                    _view.DisplayError($"На пристрої {CurrentDevice.Name} немає встановлених ігор");
                    _view.WaitForKeyPress();
                }
            }
            else
            {
                _view.DisplayError($"На пристрої {CurrentDevice.Name} вже запущена гра {CurrentDevice.CurrentlyRunningGame.Name}");
                _view.WaitForKeyPress();
            }
        }

        public void ManageGamesOnDevice()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            _gameMenu.ManageGamesOnDevice(CurrentDevice);
        }

        public void ShowInstalledGames()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            _gameMenu.ShowInstalledGames(CurrentDevice);
        }

        public void UninstallGame()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            _gameMenu.UninstallGame(CurrentDevice);
        }

        public void ManageRunningGame()
        {
            if (CurrentDevice == null)
            {
                _view.DisplayError("Спочатку виберіть пристрій.");
                _view.WaitForKeyPress();
                return;
            }

            try
            {
                if (CurrentDevice.CurrentlyRunningGame == null)
                {
                    _view.DisplayError("Немає запущеної гри.");
                    _view.WaitForKeyPress();
                    return;
                }

                _gameMenu.ManageRunningGame(CurrentDevice);
            }
            catch (Exception ex)
            {
                _view.DisplayError($"Помилка при керуванні грою: {ex.Message}");
                _view.WaitForKeyPress();
            }
        }

        public void CreateNewGame()
        {
            _gameMenu.CreateNewGame();
        }
    }
} 