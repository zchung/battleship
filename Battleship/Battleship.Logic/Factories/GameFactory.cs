
using Battleship.Data.Entities;
using Battleship.Logic.ViewModels;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Data.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using Battleship.Logic.Enums;
using Battleship.Logic.InternalModels.Interfaces;
using Battleship.Logic.InternalModels;

namespace Battleship.Logic.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IShipFactory _shipFactory;
        private readonly IRandomGeneratorFactory _randomGeneratorFactory;
        private readonly IPlayerFactory _playerFactory;
        public GameFactory(IShipFactory shipFactory, IRandomGeneratorFactory randomGeneratorFactory,
                           IPlayerFactory playerFactory)
        {
            _shipFactory = shipFactory;
            _randomGeneratorFactory = randomGeneratorFactory;
            _playerFactory = playerFactory;
        }
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
            IPlayer player = _playerFactory.GetPlayer(playerId);
            var gameViewModel =  new GameViewModel
            {
                GameId = game.GameId,
                Description = game.Description,
                CurrentPlayerIdTurn = game.CurrentPlayerIdTurn,
                PlayerId = playerId,
                Ships = player.GetShipViewModelsFromString(game),
                AttemptedCoordinates = player.GetAttemptedCoordinatesFromString(game),
                Player1Status = game.Player1Status,
                Player2Status = game.Player2Status
            };

            return gameViewModel;
        }

        public Game CreateNewGame(string description)
        {
            return new Game
            {
                Description = description,
                GameStatus = GameStatus.Active,
                Player1ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player2ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player1AttemptedCoordinatesJSON = JsonConvert.SerializeObject(new List<CoordinatesViewModel>()),
                Player2AttemptedCoordinatesJSON = JsonConvert.SerializeObject(new List<CoordinatesViewModel>()),
                Player1Status = PlayerStatus.Preparing,
                Player2Status = PlayerStatus.Empty
            };
        }

        public IGameType GetGameType(Game game, GameStatus gameStatus)
        {
            switch (gameStatus)
            {
                case GameStatus.Planning:
                    return new PlanningGame(game);
                case GameStatus.Started:
                    return new StartedGame(game, _randomGeneratorFactory);
                default:
                    return new StartedGame(game, _randomGeneratorFactory);
            }
        }
    }
}
