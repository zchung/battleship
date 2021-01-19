
using Battleship.Data.Entities;
using Battleship.Data.Models.ViewModels;
using Battleship.Logic.Factories.Interfaces;

namespace Battleship.Logic.Factories
{
    public class GameFactory : IGameFactory
    {
        public GameListViewModel GetGameListViewModel(Game game)
        {
            return new GameListViewModel
            {
                GameId = game.GameId,
                Description = game.Description
            };
        }

        public GameViewModel GetGameViewModel(Game game, int playerId)
        {
            return new GameViewModel
            {
                GameId = game.GameId,
                Description = game.Description,
                PlayerId = playerId
            };

        }
    }
}
