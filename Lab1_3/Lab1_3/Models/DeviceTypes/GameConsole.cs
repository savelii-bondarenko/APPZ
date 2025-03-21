namespace Lab1_3.Models.DeviceTypes;

public class GameConsole : Device
{
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

    public bool HasControllersConnected { get; }
    public bool HasPremiumSubscription { get; }
    public bool SupportsVR { get; }

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