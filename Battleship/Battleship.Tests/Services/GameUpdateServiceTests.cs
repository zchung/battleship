
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class GameUpdateServiceTests
    {
        private Mock<IGameDbService> _gameDbService;
        private GameUpdateService _gameUpdateService;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameUpdateService = new GameUpdateService(_gameDbService.Object);
        }
        [TestMethod]
        public async Task UpdateGameAfterPlayerJoins_Should_Correctly_Update_The_Game()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });

            var result = await _gameUpdateService.UpdateGameAfterPlayerJoins(1);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(GameStatus.Started, result.Data.GameStatus);
        }
    }
}
