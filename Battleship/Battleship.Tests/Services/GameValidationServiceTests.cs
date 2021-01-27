
using Battleship.Data.Entities;
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
    }
}
