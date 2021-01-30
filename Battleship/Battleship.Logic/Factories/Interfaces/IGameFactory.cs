
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.InternalModels.Interfaces;
using Battleship.Logic.ViewModels;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface IGameFactory
    {
        GameViewModel GetGameViewModel(Game game, int playerId);
        GameListViewModel GetGameListViewModel(Game game);
        Game CreateNewGame(string description);
        IGameType GetGameType(Game game, GameStatus gameStatus);
    }
}
