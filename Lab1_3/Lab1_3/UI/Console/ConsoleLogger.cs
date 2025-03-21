
using lab1_appz.Services;

namespace lab1_appz.UI.Console
{
    public class ConsoleLogger : IDisposable
    {
        private readonly EventSystem _eventSystem;
        

        public ConsoleLogger(EventSystem eventSystem)
        {
            _eventSystem = eventSystem;
            
            _eventSystem.SubscribeToAll(HandleEvent);
        }

        public void HandleEvent(object sender, EventData eventData)
        {
            ConsoleColor oldColor = System.Console.ForegroundColor;

            // Встановлюємо колір в залежності від типу події
            switch (eventData.Type)
            {
                case EventType.GameInstalled:
                case EventType.GameAdded:
                case EventType.GameSaved:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case EventType.GameUninstalled:
                case EventType.GameRemoved:
                    System.Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case EventType.GameStarted:
                case EventType.GameLoaded:
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case EventType.GameStopped:
                    System.Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case EventType.DeviceAdded:
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case EventType.Error:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case EventType.Information:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
            System.Console.WriteLine($"[{eventData.Type}] {eventData.Message}");
            System.Console.ForegroundColor = oldColor;
        }


        public void Dispose()
        {
            _eventSystem.UnsubscribeFromAll(HandleEvent);
        }
    }
} 