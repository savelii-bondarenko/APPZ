using System.Text.Json;
using lab1_appz.Models;

namespace lab1_appz.Services
{

    public class GameDto
    {
        public string Name { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public RequirementsDto Requirements { get; set; } = new RequirementsDto();
        public bool IsOnline { get; set; }
    }

    public class RequirementsDto
    {
        public double MinProcessorGHz { get; set; }
        public int MinRamGB { get; set; }
        public int MinVideoRamGB { get; set; }
        public int MinHddGB { get; set; }
    }

    public class GameRepository
    {
        private readonly string _filePath;
        private List<Game> _games;
        private readonly EventSystem _eventSystem;

        public GameRepository(string filePath, EventSystem eventSystem)
        {
            _filePath = filePath;
            _eventSystem = eventSystem;
            _games = new List<Game>();
            LoadGames();
        }

        public List<Game> GetAllGames()
        {
            return _games.ToList();
        }

        public Game? GetGameByName(string name)
        {
            return _games.FirstOrDefault(g => g.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public List<Game> GetGamesByGenre(GameGenre genre)
        {
            return _games.Where(g => g.Genre == genre).ToList();
        }

        public List<Game> GetOnlineGames()
        {
            return _games.Where(g => g.IsOnline).ToList();
        }

        public bool AddGame(Game game)
        {
            // Перевірка, чи гра з такою назвою вже існує
            if (_games.Any(g => g.Name.Equals(game.Name, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            _games.Add(game);
            SaveGames();
            
            _eventSystem.NotifyGameAdded(this, game);
            return true;
        }


        public bool RemoveGame(string gameName)
        {
            var game = GetGameByName(gameName);
            if (game == null)
            {
                return false;
            }

            _games.Remove(game);
            SaveGames();
            
            _eventSystem.NotifyGameRemoved(this, game);
            return true;
        }


        public bool UpdateGame(Game updatedGame)
        {
            var existingGameIndex = _games.FindIndex(g => g.Name.Equals(updatedGame.Name, StringComparison.OrdinalIgnoreCase));
            if (existingGameIndex == -1)
            {
                return false;
            }

            _games[existingGameIndex] = updatedGame;
            SaveGames();
            
            _eventSystem.NotifyGameUpdated(this, updatedGame);
            return true;
        }

        private void  LoadGames()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    string jsonText = File.ReadAllText(_filePath);
                    var gameData = JsonSerializer.Deserialize<List<GameData>>(jsonText);
                    
                    if (gameData != null)
                    {
                        _games = gameData.Select(data => new Game(
                            data.Name,
                            data.Genre,
                            new GameRequirements(
                                data.Requirements.MinProcessorGHz,
                                data.Requirements.MinRamGB,
                                data.Requirements.MinVideoRamGB,
                                data.Requirements.MinHddGB
                            ),
                            data.IsOnline
                        )).ToList();
                        _eventSystem.NotifyGamesLoaded(this, _games.Count);
                    }
                }
                else
                {
                    CreateDefaultGames();
                    SaveGames();
                }
            }
            catch (Exception ex)
            {
                _eventSystem.NotifyError(this, $"Помилка завантаження ігор: {ex.Message}");
                CreateDefaultGames();
            }
        }

        private void SaveGames()
        {
            try
            {
                var gameData = _games.Select(g => new GameData
                {
                    Name = g.Name,
                    Genre = g.Genre,
                    Requirements = new RequirementsData
                    {
                        MinProcessorGHz = g.Requirements.MinProcessorGHz,
                        MinRamGB = g.Requirements.MinRamGB,
                        MinVideoRamGB = g.Requirements.MinVideoRamGB,
                        MinHddGB = g.Requirements.MinHddGB
                    },
                    IsOnline = g.IsOnline
                }).ToList();

                string jsonText = JsonSerializer.Serialize(gameData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                
                File.WriteAllText(_filePath, jsonText);
                _eventSystem.NotifyGamesSaved(this, _games.Count);
            }
            catch (Exception ex)
            {
                _eventSystem.NotifyError(this, $"Помилка збереження ігор: {ex.Message}");
            }
        }

        private void CreateDefaultGames()
        {
            _games = new List<Game>
            {
                new Game(
                    "Farming Simulator 2023",
                    GameGenre.Simulator,
                    new GameRequirements(2.5, 4, 2, 10),
                    false
                ),
                new Game(
                    "Counter-Strike 2",
                    GameGenre.Shooter,
                    new GameRequirements(3.0, 8, 4, 50),
                    true
                ),
                new Game(
                    "World of Warcraft",
                    GameGenre.OnlineAdventure,
                    new GameRequirements(2.8, 6, 3, 100),
                    true
                ),
                new Game(
                    "Flight Simulator",
                    GameGenre.Simulator,
                    new GameRequirements(3.5, 16, 8, 150),
                    false
                ),
                new Game(
                    "Battlefield 2042",
                    GameGenre.Shooter,
                    new GameRequirements(3.2, 12, 6, 100),
                    true
                ),
                new Game(
                    "The Witcher 3",
                    GameGenre.RPG,
                    new GameRequirements(3.0, 8, 4, 50),
                    false
                ),
                new Game(
                    "FIFA 2023",
                    GameGenre.Sports,
                    new GameRequirements(2.8, 8, 3, 40),
                    true
                ),
                new Game(
                    "Civilization VI",
                    GameGenre.Strategy,
                    new GameRequirements(2.5, 6, 2, 20),
                    false
                )
            };
            
            _eventSystem.NotifyGamesLoaded(this, _games.Count);
        }
    }

    public class GameData
    {
        public string Name { get; set; } = string.Empty;
        public GameGenre Genre { get; set; }
        public RequirementsData Requirements { get; set; } = new RequirementsData();
        public bool IsOnline { get; set; }
    }

    public class RequirementsData
    {
        public double MinProcessorGHz { get; set; }
        public int MinRamGB { get; set; }
        public int MinVideoRamGB { get; set; }
        public int MinHddGB { get; set; }
    }
} 