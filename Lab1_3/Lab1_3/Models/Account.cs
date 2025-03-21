namespace lab1_appz.Models
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public bool IsPremium { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private readonly List<Game> _availableGames = new();

        public Account(string username, bool isPremium = false)
        {
            Id = Guid.NewGuid();
            Username = username;
            IsPremium = isPremium;
            CreatedAt = DateTime.Now;
        }


        public bool AddGame(Game game)
        {
            if (!_availableGames.Contains(game))
            {
                _availableGames.Add(game);
                return true;
            }
            return false;
        }

        public bool RemoveGame(Game game)
        {
            return _availableGames.Remove(game);
        }

        public List<Game> GetAvailableGames()
        {
            return new List<Game>(_availableGames);
        }


        public bool HasAccessToGame(Game game)
        {
            return _availableGames.Contains(game) || IsPremium;
        }

        public void UpdatePremiumStatus(bool isPremium)
        {
            IsPremium = isPremium;
        }

        public override string ToString()
        {
            return $"{Username}{(IsPremium ? " (Преміум)" : "")} - {_availableGames.Count} доступних ігор";
        }
    }
} 