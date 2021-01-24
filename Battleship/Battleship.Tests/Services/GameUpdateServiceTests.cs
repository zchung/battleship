
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class GameUpdateServiceTests
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IGameFactory> _gameFactory;
        private GameUpdateService _gameUpdateService;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameFactory = new Mock<IGameFactory>();
            _gameUpdateService = new GameUpdateService(_gameDbService.Object, _gameFactory.Object);
        }
        [TestMethod]
        public async Task UpdateGameAfterPlayerJoins_Should_Correctly_Update_The_Game()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });

            var result = await _gameUpdateService.UpdateGameAfterPlayerJoins(1);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(GameStatus.Planning, result.Data.GameStatus);
            Assert.AreEqual(PlayerStatus.Preparing, result.Data.Player2Status);
        }

        [TestMethod]
        public async Task UpdatePlayerToReady_Should_Update_The_Status_If_The_Condition_Is_Met()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel 
            { 
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        { 
                            new CoordinatesViewModel(1, 1),
                            new CoordinatesViewModel(1, 2)
                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 1);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task UpdatePlayerToPrepared_Should_Not_Update_The_Status_If_The_Condition_Is_Not_Met()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel
            {
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        {

                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 1);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Never);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public async Task UpdateGameStatus_Should_Correctly_Update_The_Status()
        {
            Game game = new Game();
            GameStatus gameStatus = GameStatus.Started;
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdateGameStatus(1, gameStatus);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(gameStatus, game.GameStatus);
        }
    }
}
