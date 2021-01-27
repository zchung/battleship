
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class GameValidationServiceTests
    {
        private Mock<IGameDbService> _gameDbService;
        private IGameValidationService _gameValidationService;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameValidationService = new GameValidationService(_gameDbService.Object);

        }
        [TestMethod]
        public void CanAttackPlayer_Should_Return_Success_If_Can_Attack_Player()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game { CurrentPlayerIdTurn = 1 }, Success = true });

            var result = _gameValidationService.CanAttackPlayer(1, 1);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void CanAttackplayer_Should_Return_False_If_Cannot_Attack_Player()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game { CurrentPlayerIdTurn = 2 }, Success = true });

            var result = _gameValidationService.CanAttackPlayer(1, 1);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CanAttackplayer_Should_Return_False_If_Getting_Game_Fails()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Success = false, Message = "unable to get" });

            var result = _gameValidationService.CanAttackPlayer(1, 1);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CanAttackplayer_Should_Return_False_If_Getting_Game_Is_Completed()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game { GameStatus = GameStatus.Completed }, Success = true});

            var result = _gameValidationService.CanAttackPlayer(1, 1);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CanJoinGame_Should_Return_True_If_The_Game_Is_In_The_Correct_Status()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game { GameStatus = GameStatus.Active }, Success = true });

            var result = _gameValidationService.CanJoinGame(1);

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void CanJoinGame_Should_Return_False_If_The_Game_Is_In_The_Incorrect_Status()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game { GameStatus = GameStatus.Planning }, Success = true });

            var result = _gameValidationService.CanJoinGame(1);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void CanJoinGame_Should_Return_False_If_The_Game_Cannot_Be_Found()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Success = false, Message = "Game Not found" });

            var result = _gameValidationService.CanJoinGame(1);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }
    }
}
