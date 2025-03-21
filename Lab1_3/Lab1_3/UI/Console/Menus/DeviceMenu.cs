using lab1_appz.Models;
using lab1_appz.Services;

namespace lab1_appz.UI.Console.Menus
{

    public class DeviceMenu
    {
        private readonly IConsoleView _view;
        private readonly ValidationService _validationService;
        private readonly List<Device> _devices;
        private readonly EventSystem _eventSystem;

        public DeviceMenu(IConsoleView view, ValidationService validationService, List<Device> devices, EventSystem eventSystem)
        {
            _view = view;
            _validationService = validationService;
            _devices = devices;
            _eventSystem = eventSystem;
        }

        public void ShowDevices()
        {
            _view.ClearScreen();
            _view.DisplayHeader("Список пристроїв");
            
            if (_devices.Count == 0)
            {
                _view.DisplayInfo("Немає доступних пристроїв. Додайте новий пристрій.");
                _view.WaitForKeyPress();
                return;
            }
            
            for (int i = 0; i < _devices.Count; i++)
            {
                _view.DisplayInfo($"{i + 1}. {_devices[i].ToString()}");
            }
            
            _view.WaitForKeyPress();
        }

        public Device? SelectDevice()
        {
            _view.ClearScreen();
            _view.DisplayHeader("Вибір пристрою");
            
            if (_devices.Count == 0)
            {
                _view.DisplayInfo("Немає доступних пристроїв. Додайте новий пристрій.");
                _view.WaitForKeyPress();
                return null;
            }
            
            // Показуємо список пристроїв
            for (int i = 0; i < _devices.Count; i++)
            {
                _view.DisplayInfo($"{i + 1}. {_devices[i].Name} ({_devices[i].GetDeviceType()})");
            }
            
            string input = _view.GetUserInput("Виберіть номер пристрою (0 для повернення)");
            
            if (!_validationService.IsValidMenuOption(input, 0, _devices.Count, out int deviceIndex))
            {
                _view.DisplayError($"Невірний номер пристрою. Будь ласка, введіть число від 0 до {_devices.Count}");
                _view.WaitForKeyPress();
                return null;
            }
            
            if (deviceIndex == 0)
            {
                return null;
            }
            
            // Зсув індексу на 1, оскільки в меню нумерація з 1
            Device selectedDevice = _devices[deviceIndex - 1];
            _view.DisplaySuccess($"Вибрано пристрій: {selectedDevice.Name}");
            _view.WaitForKeyPress();
            
            return selectedDevice;
        }
        

        public Device? AddDevice()
        {
            _view.ClearScreen();
            _view.DisplayHeader("Додавання нового пристрою");
            
            string[] deviceTypes = { "Настільний комп'ютер", "Ноутбук", "Ігрова консоль", "Мобільний телефон" };
            
            _view.DisplayInfo("Виберіть тип пристрою:");
            _view.DisplayMenu(deviceTypes);
            
            string input = _view.GetUserInput("Виберіть тип пристрою (0 для повернення)");
            
            if (!_validationService.TryParseIntInRange(input, 0, deviceTypes.Length, out int deviceTypeIndex))
            {
                _view.DisplayError("Невірний тип пристрою.");
                _view.WaitForKeyPress();
                return null;
            }
            
            if (deviceTypeIndex == 0)
            {
                return null;
            }
            
            // Загальні параметри для всіх пристроїв
            string name = _view.GetUserInput("Введіть назву пристрою");
            if (!_validationService.IsValidName(name))
            {
                _view.DisplayError(ValidationService.ValidationErrors["name_invalid"]);
                _view.WaitForKeyPress();
                return null;
            }
            
            double processorGHz = 0;
            int ramGB = 0, videoRamGB = 0, hddGB = 0;
            
            // Запит на введення технічних характеристик
            string processorInput = _view.GetUserInput("Введіть частоту процесора (GHz, від 1.0 до 5.0)");
            if (!_validationService.TryParseDoubleInRange(processorInput, 1.0, 5.0, out processorGHz))
            {
                _view.DisplayError(ValidationService.ValidationErrors["decimal_invalid"]);
                _view.WaitForKeyPress();
                return null;
            }
            
            string ramInput = _view.GetUserInput("Введіть об'єм оперативної пам'яті (GB, від 1 до 64)");
            if (!_validationService.TryParseIntInRange(ramInput, 1, 64, out ramGB))
            {
                _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
                _view.WaitForKeyPress();
                return null;
            }
            
            string videoRamInput = _view.GetUserInput("Введіть об'єм відеопам'яті (GB, від 0 до 16)");
            if (!_validationService.TryParseIntInRange(videoRamInput, 0, 16, out videoRamGB))
            {
                _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
                _view.WaitForKeyPress();
                return null;
            }
            
            string hddInput = _view.GetUserInput("Введіть об'єм жорсткого диска (GB, від 16 до 2000)");
            if (!_validationService.TryParseIntInRange(hddInput, 16, 2000, out hddGB))
            {
                _view.DisplayError(ValidationService.ValidationErrors["number_invalid"]);
                _view.WaitForKeyPress();
                return null;
            }
            
            Device? device = null;
            
            // Створення відповідного типу пристрою
            switch (deviceTypeIndex)
            {
                case 1: // Настільний комп'ютер
                    bool hasSpeakers = false;
                    bool hasHighEndGraphicsCard = false;
                    
                    string speakersInput = _view.GetUserInput("Чи є динаміки? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(speakersInput, out hasSpeakers))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string graphicsInput = _view.GetUserInput("Чи є потужна відеокарта? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(graphicsInput, out hasHighEndGraphicsCard))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    device = new DesktopComputer(name, processorGHz, ramGB, videoRamGB, hddGB, hasSpeakers, hasHighEndGraphicsCard);
                    break;
                    
                case 2: // Ноутбук
                    double batteryLifeHours = 0;
                    bool hasTouchScreen = false;
                    bool hasWifi = false;
                    
                    string batteryInput = _view.GetUserInput("Введіть час автономної роботи (години, від 1 до 24)");
                    if (!_validationService.TryParseDoubleInRange(batteryInput, 1, 24, out batteryLifeHours))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["decimal_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string touchInput = _view.GetUserInput("Чи є сенсорний екран? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(touchInput, out hasTouchScreen))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string wifiInput = _view.GetUserInput("Чи є Wi-Fi? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(wifiInput, out hasWifi))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    device = new Laptop(name, processorGHz, ramGB, videoRamGB, hddGB, batteryLifeHours, hasTouchScreen, hasWifi);
                    break;
                    
                case 3: // Ігрова консоль
                    bool hasControllersConnected = false;
                    bool hasPremiumSubscription = false;
                    bool supportsVR = false;
                    
                    string controllersInput = _view.GetUserInput("Чи підключені контролери? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(controllersInput, out hasControllersConnected))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string premiumInput = _view.GetUserInput("Чи є преміум підписка? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(premiumInput, out hasPremiumSubscription))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string vrInput = _view.GetUserInput("Чи підтримує VR? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(vrInput, out supportsVR))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    device = new GameConsole(name, processorGHz, ramGB, videoRamGB, hddGB, hasControllersConnected, hasPremiumSubscription, supportsVR);
                    break;
                    
                case 4: // Мобільний телефон
                    string operatingSystem = "";
                    bool hasCellularData = false;
                    double screenSize = 0;
                    
                    operatingSystem = _view.GetUserInput("Введіть операційну систему (Android/iOS)");
                    if (string.IsNullOrWhiteSpace(operatingSystem))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["name_required"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string cellularInput = _view.GetUserInput("Чи є мобільний інтернет? (Так/Ні)");
                    if (!_validationService.TryParseBoolean(cellularInput, out hasCellularData))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["boolean_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    string screenInput = _view.GetUserInput("Введіть розмір екрану (дюйми, від 3 до 10)");
                    if (!_validationService.TryParseDoubleInRange(screenInput, 3, 10, out screenSize))
                    {
                        _view.DisplayError(ValidationService.ValidationErrors["decimal_invalid"]);
                        _view.WaitForKeyPress();
                        return null;
                    }
                    
                    device = new MobilePhone(name, processorGHz, ramGB, videoRamGB, hddGB, operatingSystem, hasCellularData, screenSize);
                    break;
            }
            
            if (device != null)
            {
                _devices.Add(device);
                _eventSystem.NotifyDeviceAdded(this, device);
                _view.DisplaySuccess($"Пристрій {device.Name} успішно додано!");
                _view.WaitForKeyPress();
            }
            
            return device;
        }
    }
} 