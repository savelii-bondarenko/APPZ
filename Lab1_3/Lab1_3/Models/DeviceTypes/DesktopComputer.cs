using Lab1_3.enums;

namespace Lab1_3.Models.DeviceTypes;

public class DesktopComputer : Device
{
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

    public bool HasSpeakers { get; }
    public bool HasHighEndGraphicsCard { get; }

    public override bool CanRunGame(Game game)
    {
        var baseCheck = base.CanRunGame(game);

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