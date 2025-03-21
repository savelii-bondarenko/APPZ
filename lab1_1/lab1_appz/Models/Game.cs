namespace lab1_appz.Models
{
    public enum GameGenre
    {
        Simulator,
        Shooter,
        OnlineAdventure,
        RPG,
        Strategy,
        Sports
    }

    public class GameRequirements
    {
        public double MinProcessorGHz { get; set; }
        public int MinRamGB { get; set; }
        public int MinVideoRamGB { get; set; }
        public int MinHddGB { get; set; }

        public GameRequirements(double minProcessorGHz, int minRamGB, int minVideoRamGB, int minHddGB)
        {
            MinProcessorGHz = minProcessorGHz;
            MinRamGB = minRamGB;
            MinVideoRamGB = minVideoRamGB;
            MinHddGB = minHddGB;
        }

        public override string ToString()
        {
            return $"Процесор: {MinProcessorGHz} GHz, RAM: {MinRamGB} GB, VRAM: {MinVideoRamGB} GB, HDD: {MinHddGB} GB";
        }
    }

    public class Game
    {
        public string Name { get; private set; }
        public GameGenre Genre { get; private set; }
        public GameRequirements Requirements { get; private set; }
        public bool IsOnline { get; private set; }
        public bool IsInstalled { get; private set; }
        public int Rating { get; private set; }
        public bool IsRunning { get; private set; }
        public string? SavedState { get; private set; }

        public Game(string name, GameGenre genre, GameRequirements requirements, bool isOnline)
        {
            Name = name;
            Genre = genre;
            Requirements = requirements;
            IsOnline = isOnline;
            IsInstalled = false;
            Rating = 0;
            IsRunning = false;
            SavedState = null;
        }

        public bool Install()
        {
            if (IsInstalled)
                return true;

            IsInstalled = true;
            return true;
        }

        public int Uninstall()
        {
            if (!IsInstalled)
                return 0;
                
            int freedSpace = Requirements.MinHddGB;
            IsInstalled = false;
            IsRunning = false;
            SavedState = null;
            
            return freedSpace;
        }

        public bool Run(bool hasAccount, bool hasControllers = true)
        {
            // Онлайн пригоди вимагають облікового запису
            if (IsOnline && !hasAccount)
                return false;

            // Якщо гра вже запущена, то ще раз запустити не можна
            if (IsRunning)
                return false;

            // Для консолі потрібні маніпулятори
            if (!hasControllers)
                return false;

            IsRunning = true;
            
            return true;
        }

        public void Stop()
        {
            IsRunning = false;
        }


        public bool Save(string state)
        {
            if (!IsRunning)
                return false;
                
            SavedState = state;
            
            return true;
        }


        public string? Load()
        {
            if (!IsRunning || SavedState == null)
                return null;
                
            return SavedState;
        }

        public bool SetRating(int rating)
        {
            if (rating < 1 || rating > 5)
                return false;
                
            Rating = rating;
            
            return true;
        }

        public override string ToString()
        {
            return $"{Name} ({Genre}), {Requirements}, Online: {(IsOnline ? "Так" : "Ні")}, " +
                   $"Встановлено: {(IsInstalled ? "Так" : "Ні")}, Рейтинг: {Rating}";
        }
    }
} 