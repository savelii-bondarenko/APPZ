namespace Lab1_3.Models;

public abstract class Device
{
    protected Device(string name, double processorGHz, int ramGB, int videoRamGB, int hddGB)
    {
        Name = name;
        ProcessorGHz = processorGHz;
        RamGB = ramGB;
        VideoRamGB = videoRamGB;
        HddGB = hddGB;
        FreeHddGB = hddGB; // На початку весь диск вільний
        InstalledGames = new List<Game>();
        CurrentlyRunningGame = null;
        MaxGamesCount = 10;
    }

    public string Name { get; protected set; }
    public double ProcessorGHz { get; protected set; }
    public int RamGB { get; protected set; }
    public int VideoRamGB { get; protected set; }
    public int HddGB { get; protected set; }
    public int FreeHddGB { get; protected set; }
    public List<Game> InstalledGames { get; protected set; }
    public Game? CurrentlyRunningGame { get; protected set; }

    public int MaxGamesCount { get; protected set; }

    public virtual bool CanRunGame(Game game)
    {
        return ProcessorGHz >= game.Requirements.MinProcessorGHz &&
               RamGB >= game.Requirements.MinRamGB &&
               VideoRamGB >= game.Requirements.MinVideoRamGB;
    }

    public virtual bool InstallGame(Game game)
    {
        if (game.IsInstalled)
            return false;

        if (InstalledGames.Count >= MaxGamesCount)
            return false;

        if (FreeHddGB < game.Requirements.MinHddGB)
            return false;

        if (!CanRunGame(game))
            return false;

        if (game.Install())
        {
            InstalledGames.Add(game);
            FreeHddGB -= game.Requirements.MinHddGB;
            return true;
        }

        return false;
    }

    public virtual bool UninstallGame(Game game)
    {
        if (!InstalledGames.Contains(game))
            return false;

        // Видалення гри
        var freedSpace = game.Uninstall();
        if (freedSpace > 0)
        {
            InstalledGames.Remove(game);
            FreeHddGB += freedSpace;

            if (CurrentlyRunningGame == game) CurrentlyRunningGame = null;

            return true;
        }

        return false;
    }

    public virtual bool RunGame(Game game, bool hasAccount)
    {
        if (CurrentlyRunningGame != null)
            return false;

        if (!InstalledGames.Contains(game))
            return false;

        if (game.Run(hasAccount))
        {
            CurrentlyRunningGame = game;
            return true;
        }

        return false;
    }

    // Зупинка поточної гри
    public virtual bool StopGame()
    {
        if (CurrentlyRunningGame == null)
            return false;

        CurrentlyRunningGame.Stop();
        CurrentlyRunningGame = null;
        return true;
    }

    public virtual bool SaveGame(string state)
    {
        if (CurrentlyRunningGame == null)
            return false;

        return CurrentlyRunningGame.Save(state);
    }

    public virtual string? LoadGame()
    {
        if (CurrentlyRunningGame == null)
            return null;

        return CurrentlyRunningGame.Load();
    }

    public List<Game> GetInstalledGames()
    {
        return InstalledGames.ToList();
    }

    public abstract string GetDeviceType();

    public override string ToString()
    {
        return $"{Name} ({GetDeviceType()}), " +
               $"Процесор: {ProcessorGHz} GHz, " +
               $"RAM: {RamGB} GB, " +
               $"VRAM: {VideoRamGB} GB, " +
               $"HDD: {HddGB} GB (вільно: {FreeHddGB} GB), " +
               $"Ігри: {InstalledGames.Count}/{MaxGamesCount}";
    }
}