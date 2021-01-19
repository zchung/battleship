
using Battleship.Data.Entities;
using Battleship.Data.Models.ViewModels;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface IGameFactory
    {
        GameViewModel GetGameViewModel(Game game, int playerId);
        GameListViewModel GetGameListViewModel(Game game);
    }
}
