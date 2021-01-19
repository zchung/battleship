
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.Factories
{
    [TestClass]
    public class GameFactoryTests
    {
        private IGameFactory _gameFactory;

        [TestInitialize]
        public void Initialise()
        {
            _gameFactory = new GameFactory();
        }

        [TestMethod]
        public void GetGameViewModel_Should_Return_A_Valid_ViewModel()
        {
            Game game = new Game
            {
                GameId = 1,
                Description = "test game"
            };
            var result = _gameFactory.GetGameViewModel(game, 1);

            Assert.AreEqual(game.GameId, result.GameId);
            Assert.AreEqual(game.Description, result.Description);
            Assert.AreEqual(1, result.PlayerId);
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
    }
}
