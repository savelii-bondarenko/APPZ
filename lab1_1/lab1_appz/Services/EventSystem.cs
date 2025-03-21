using lab1_appz.Models;

namespace lab1_appz.Services
{
    public enum EventType
    {
        GameInstalled,
        GameUninstalled,
        GameStarted,
        GameStopped,
        GameSaved,
        GameLoaded,
        GameError,
        DeviceAdded,
        GameAdded,
        GameRemoved,
        GameUpdated,
        GamesLoaded,
        GamesSaved,
        Information,
        Error
    }

    public class EventData
    {
        public EventType Type { get; set; }
        public string Message { get; set; }
        public object? Sender { get; set; }
        public object? Data { get; set; }

        public EventData(EventType type, string message, object? sender = null, object? data = null)
        {
            Type = type;
            Message = message;
            Sender = sender;
            Data = data;
        }
    }


    public delegate void EventHandler(object sender, EventData eventData);
    public class EventSystem
    {
        private static EventSystem? _instance;
        
        private readonly Dictionary<EventType, List<EventHandler>> _subscribers = new();
        public static EventSystem Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = new EventSystem();
                }
                return _instance;
            } 
        }

        public EventSystem()
        {
            foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
            {
                _subscribers[eventType] = new List<EventHandler>();
            }
        }
        public void Subscribe(EventType eventType, EventHandler handler)
        {
            if (!_subscribers[eventType].Contains(handler))
            {
                _subscribers[eventType].Add(handler);
            }
        }

        public void Unsubscribe(EventType eventType, EventHandler handler)
        {
            if (_subscribers[eventType].Contains(handler))
            {
                _subscribers[eventType].Remove(handler);
            }
        }

        public void SubscribeToAll(EventHandler handler)
        {
            foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
            {
                Subscribe(eventType, handler);
            }
        }

        public void UnsubscribeFromAll(EventHandler handler)
        {
            foreach (EventType eventType in Enum.GetValues(typeof(EventType)))
            {
                Unsubscribe(eventType, handler);
            }
        }

        private void Notify(object sender, EventData eventData)
        {
            foreach (var handler in _subscribers[eventData.Type].ToList())
            {
                try
                {
                    handler(sender, eventData);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка в обробнику події {eventData.Type}: {ex.Message}");
                }
            }
        }

        public void NotifyGameInstalled(object sender, Game game, Device device)
        {
            Notify(sender, new EventData(
                EventType.GameInstalled,
                $"Гру '{game.Name}' встановлено на пристрій '{device.Name}'",
                sender,
                new { Game = game, Device = device }
            ));
        }


        public void NotifyGameUninstalled(object sender, Game game, Device device)
        {
            Notify(sender, new EventData(
                EventType.GameUninstalled,
                $"Гру '{game.Name}' видалено з пристрою '{device.Name}'",
                sender,
                new { Game = game, Device = device }
            ));
        }


        public void NotifyGameStarted(object sender, Game game, Device device)
        {
            Notify(sender, new EventData(
                EventType.GameStarted,
                $"Гру '{game.Name}' запущено на пристрої '{device.Name}'",
                sender,
                new { Game = game, Device = device }
            ));
        }


        public void NotifyGameStopped(object sender, Game game, Device device)
        {
            Notify(sender, new EventData(
                EventType.GameStopped,
                $"Гру '{game.Name}' зупинено на пристрої '{device.Name}'",
                sender,
                new { Game = game, Device = device }
            ));
        }

        public void NotifyGameSaved(object sender, Game game)
        {
            Notify(sender, new EventData(
                EventType.GameSaved,
                $"Стан гри '{game.Name}' збережено",
                sender,
                game
            ));
        }

        public void NotifyGameLoaded(object sender, Game game)
        {
            Notify(sender, new EventData(
                EventType.GameLoaded,
                $"Стан гри '{game.Name}' завантажено",
                sender,
                game
            ));
        }

        public void NotifyDeviceAdded(object sender, Device device)
        {
            Notify(sender, new EventData(
                EventType.DeviceAdded,
                $"Додано новий пристрій: '{device.Name}' ({device.GetDeviceType()})",
                sender,
                device
            ));
        }

        public void NotifyGameAdded(object sender, Game game)
        {
            Notify(sender, new EventData(
                EventType.GameAdded,
                $"Додано нову гру: '{game.Name}' ({game.Genre})",
                sender,
                game
            ));
        }

        public void NotifyGameRemoved(object sender, Game game)
        {
            Notify(sender, new EventData(
                EventType.GameRemoved,
                $"Видалено гру: '{game.Name}'",
                sender,
                game
            ));
        }

        public void NotifyGameUpdated(object sender, Game game)
        {
            Notify(sender, new EventData(
                EventType.GameUpdated,
                $"Оновлено інформацію про гру: '{game.Name}'",
                sender,
                game
            ));
        }

        public void NotifyGamesLoaded(object sender, int count)
        {
            Notify(sender, new EventData(
                EventType.GamesLoaded,
                $"Завантажено {count} ігор",
                sender,
                count
            ));
        }

        public void NotifyGamesSaved(object sender, int count)
        {
            Notify(sender, new EventData(
                EventType.GamesSaved,
                $"Збережено {count} ігор",
                sender,
                count
            ));
        }

        public void NotifyInformation(object sender, string message)
        {
            Notify(sender, new EventData(
                EventType.Information,
                message,
                sender
            ));
        }

        public void NotifyError(object sender, string message)
        {
            Notify(sender, new EventData(
                EventType.Error,
                message,
                sender
            ));
        }
    }
} 