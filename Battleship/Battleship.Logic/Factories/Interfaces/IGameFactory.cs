
using Battleship.Data.Entities;
using Battleship.Logic.ViewModels;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface IGameFactory
    {
        GameViewModel GetGameViewModel(Game game, int playerId);
        List GetGameListViewModel(Game game);
        Game CreateNewGame(string description);
    }
}
