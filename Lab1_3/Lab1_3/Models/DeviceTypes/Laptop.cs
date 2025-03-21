namespace Lab1_3.Models.DeviceTypes;

public class Laptop : Device
{
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

    public double BatteryLifeHours { get; }
    public bool HasTouchScreen { get; }
    public bool HasWifi { get; }

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