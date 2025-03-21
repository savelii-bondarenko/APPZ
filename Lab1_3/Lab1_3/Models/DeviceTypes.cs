namespace lab1_appz.Models
{
    public class DesktopComputer : Device
    {
        public bool HasSpeakers { get; private set; }
        public bool HasHighEndGraphicsCard { get; private set; }

        public DesktopComputer(
            string name,
            double processorGHz,
            int ramGB,
            int videoRamGB,
            int hddGB,
            bool hasSpeakers,
            bool hasHighEndGraphicsCard)
            : base(name, processorGHz, ramGB, videoRamGB, hddGB)
        {
            HasSpeakers = hasSpeakers;
            HasHighEndGraphicsCard = hasHighEndGraphicsCard;
            MaxGamesCount = 20; // Настільний комп'ютер може мати більше ігор
        }

        public override bool CanRunGame(Game game)
        {
            bool baseCheck = base.CanRunGame(game);
            
            // Додаткова перевірка для шутерів - потрібна високопродуктивна відеокарта
            if (game.Genre == GameGenre.Shooter && !HasHighEndGraphicsCard)
                return false;
                
            return baseCheck;
        }

        public override string GetDeviceType()
        {
            return "ПК";
        }

        public override string ToString()
        {
            return base.ToString() + $", Динаміки: {(HasSpeakers ? "Є" : "Немає")}, " +
                   $"Потужна відеокарта: {(HasHighEndGraphicsCard ? "Є" : "Немає")}";
        }
    }
    public class Laptop : Device
    {
        public double BatteryLifeHours { get; private set; }
        public bool HasTouchScreen { get; private set; }
        public bool HasWifi { get; private set; }

        public Laptop(
            string name, 
            double processorGHz, 
            int ramGB, 
            int videoRamGB, 
            int hddGB, 
            double batteryLifeHours, 
            bool hasTouchScreen, 
            bool hasWifi) 
            : base(name, processorGHz, ramGB, videoRamGB, hddGB)
        {
            BatteryLifeHours = batteryLifeHours;
            HasTouchScreen = hasTouchScreen;
            HasWifi = hasWifi;
            MaxGamesCount = 15; // Ноутбук може мати менше ігор ніж настільний ПК
        }

        public override bool RunGame(Game game, bool hasAccount)
        {
            // Перевірка на онлайн-гру і наявність Wi-Fi
            if (game.IsOnline && !HasWifi)
                return false;
                
            return base.RunGame(game, hasAccount);
        }

        public override string GetDeviceType()
        {
            return "Ноутбук";
        }

        public override string ToString()
        {
            return base.ToString() + $", Батарея: {BatteryLifeHours} годин, " +
                   $"Сенсорний екран: {(HasTouchScreen ? "Є" : "Немає")}, " +
                   $"Wi-Fi: {(HasWifi ? "Є" : "Немає")}";
        }
    }
    public class GameConsole : Device
    {
        public bool HasControllersConnected { get; private set; }
        public bool HasPremiumSubscription { get; private set; }
        public bool SupportsVR { get; private set; }

        public GameConsole(
            string name, 
            double processorGHz, 
            int ramGB, 
            int videoRamGB, 
            int hddGB, 
            bool hasControllersConnected, 
            bool hasPremiumSubscription, 
            bool supportsVR) 
            : base(name, processorGHz, ramGB, videoRamGB, hddGB)
        {
            HasControllersConnected = hasControllersConnected;
            HasPremiumSubscription = hasPremiumSubscription;
            SupportsVR = supportsVR;
            MaxGamesCount = 10; // Консоль має обмежену кількість ігор
        }

        public override bool RunGame(Game game, bool hasAccount)
        {
            // Для консолі необхідні контролери
            if (!HasControllersConnected)
                return false;
                
            // Онлайн-ігри вимагають преміум-підписки
            if (game.IsOnline && !HasPremiumSubscription)
                return false;
                
            return base.RunGame(game, hasAccount);
        }

        public override string GetDeviceType()
        {
            return "Ігрова консоль";
        }

        public override string ToString()
        {
            return base.ToString() + $", Контролери: {(HasControllersConnected ? "Підключені" : "Відключені")}, " +
                   $"Преміум підписка: {(HasPremiumSubscription ? "Є" : "Немає")}, " +
                   $"Підтримка VR: {(SupportsVR ? "Є" : "Немає")}";
        }
    }
    public class MobilePhone : Device
    {
        public string OperatingSystem { get; private set; }
        public bool HasCellularData { get; private set; }
        public double ScreenSize { get; private set; }

        public MobilePhone(
            string name, 
            double processorGHz, 
            int ramGB, 
            int videoRamGB, 
            int hddGB, 
            string operatingSystem, 
            bool hasCellularData, 
            double screenSize) 
            : base(name, processorGHz, ramGB, videoRamGB, hddGB)
        {
            OperatingSystem = operatingSystem;
            HasCellularData = hasCellularData;
            ScreenSize = screenSize;
            MaxGamesCount = 30; // Мобільні телефони можуть мати багато маленьких ігор
        }

        public override bool CanRunGame(Game game)
        {
            bool baseCheck = base.CanRunGame(game);
            
            // Обмеження для великих ігор на маленьких екранах
            if (game.Genre == GameGenre.Shooter && ScreenSize < 5.0)
                return false;
                
            return baseCheck;
        }

        public override bool RunGame(Game game, bool hasAccount)
        {
            // Для мобільних онлайн-ігор потрібен або Wi-Fi, або стільниковий зв'язок
            if (game.IsOnline && !HasCellularData)
                return false;
                
            return base.RunGame(game, hasAccount);
        }

        public override string GetDeviceType()
        {
            return "Мобільний телефон";
        }

        public override string ToString()
        {
            return base.ToString() + $", ОС: {OperatingSystem}, " +
                   $"Мобільні дані: {(HasCellularData ? "Є" : "Немає")}, " +
                   $"Розмір екрану: {ScreenSize} дюймів";
        }
    }
} 