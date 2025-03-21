using Lab1_3.enums;

namespace Lab1_3.Models.DeviceTypes;

public class MobilePhone : Device
{
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

    public string OperatingSystem { get; }
    public bool HasCellularData { get; }
    public double ScreenSize { get; }

    public override bool CanRunGame(Game game)
    {
        var baseCheck = base.CanRunGame(game);

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