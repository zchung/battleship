
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Battleship.Tests.Factories
{
    [TestClass]
    public class GameFactoryTests
    {
        private IShipFactory _shipFactory;
        private IRandomGeneratorFactory _randomGeneratorFactory;
        private IGameFactory _gameFactory;
        private IPlayerFactory _playerFactory;

        [TestInitialize]
        public void Initialise()
        {
            _shipFactory = new ShipFactory(); // Not mocking because the values from here are static.
            _randomGeneratorFactory = new RandomGeneratorFactory();
            _playerFactory = new PlayerFactory();
            _gameFactory = new GameFactory(_shipFactory, _randomGeneratorFactory, _playerFactory);
        }

        [TestMethod]
        public void GetGameViewModel_Should_Return_A_Valid_ViewModel()
        {
            Game game = new Game
            {
                GameId = 1,
                Description = "test game",
                Player1ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player2ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player1AttemptedCoordinatesJSON = JsonConvert.SerializeObject(new List<CoordinatesViewModel> { new CoordinatesViewModel()}),
                Player2AttemptedCoordinatesJSON = JsonConvert.SerializeObject(new List<CoordinatesViewModel> { new CoordinatesViewModel()})
            };
            var result = _gameFactory.GetGameViewModel(game, 1);

            Assert.AreEqual(game.GameId, result.GameId);
            Assert.AreEqual(game.Description, result.Description);
            Assert.AreEqual(1, result.PlayerId);
            Assert.IsNotNull(result.Ships);

            result = _gameFactory.GetGameViewModel(game, 2);

            Assert.AreEqual(game.GameId, result.GameId);
            Assert.AreEqual(game.Description, result.Description);
            Assert.AreEqual(2, result.PlayerId);
            Assert.IsNotNull(result.Ships);
        }

        [TestMethod]
        public void GetGameListViewModel_Should_Return_A_Valid_ViewModel()
        {
            Game game = new Game
            {
                GameId = 1,
                Description = "test game"
            };
            var result = _gameFactory.GetGameListViewModel(game);

            Assert.AreEqual(game.GameId, result.GameId);
            Assert.AreEqual(game.Description, result.Description);
        }

        [TestMethod]
        public void CreateNewGame_Should_Return_A_New_Game()
        {
            string description = "test";
            var result = _gameFactory.CreateNewGame(description);

            Assert.IsNotNull(result);
            Assert.AreEqual(description, result.Description);
            Assert.IsNotNull(result.Player1ShipsJSON);
            Assert.IsNotNull(result.Player2ShipsJSON);
        }

        [TestMethod]
        public void GetGameType_Should_Return_The_Type()
        {
            var result = _gameFactory.GetGameType(new Game(), GameStatus.Planning);

            Assert.IsInstanceOfType(result, typeof(PlanningGame));

            result = _gameFactory.GetGameType(new Game(), GameStatus.Started);

            Assert.IsInstanceOfType(result, typeof(StartedGame));
        }
    }
}
