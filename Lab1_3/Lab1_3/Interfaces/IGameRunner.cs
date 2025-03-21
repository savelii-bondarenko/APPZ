using Lab1_3.Models;

namespace Lab1_3.Interfaces;

public interface IGameRunner
{
    bool CanRunGame(Game game);
    bool InstallGame(Game game);
    void FreeSpace(int gb);
}