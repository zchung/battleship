using Battleship.Controllers;
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Battleship.Models.Requests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Tests.Controllers
{
    [TestClass]
    public class ShipControllerTests : ControllerTestBase
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IGameFactory> _gameFactory;
        private Mock<IShipUpdateService> _shipUpdateService;
        private IShipController _shipController;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameFactory = new Mock<IGameFactory>();
            _shipUpdateService = new Mock<IShipUpdateService>();
            _shipController = new ShipController(_gameDbService.Object, _gameFactory.Object, _shipUpdateService.Object);
        }
        [TestMethod]
        public async Task UpdateShipPosition_Should_Invoke_The_Correct_Methods()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(new GameViewModel { Ships = new List<ShipViewModel> { new ShipViewModel { ShipType = ShipType.Cruiser } } });
            _shipUpdateService.Setup(s => s.UpdateShipPosition(It.IsAny<List<ShipViewModel>>(), It.IsAny<CoordinatesViewModel>(), It.IsAny<ShipType>(), It.IsAny<ShipOrientationType>())).Returns(new Result { Success = true });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _shipController.UpdateShipPosition(new UpdateShipPositionRequest { GameId = 1, PlayerId = 1, ShipOrientationType = ShipOrientationType.HorizontalRight, ShipType = ShipType.Cruiser, StartPosition = new CoordinatesViewModel(1, 1) });

            var castResult = ValidateOkResult<Result<ShipViewModel>>(result);
            Assert.IsTrue(castResult.Success);
            Assert.IsNotNull(castResult.Data);

            result = await _shipController.UpdateShipPosition(new UpdateShipPositionRequest { GameId = 1, PlayerId = 2, ShipOrientationType = ShipOrientationType.HorizontalRight, ShipType = ShipType.Cruiser, StartPosition = new CoordinatesViewModel(1, 1) });

            castResult = ValidateOkResult<Result<ShipViewModel>>(result);
            Assert.IsTrue(castResult.Success);
            Assert.IsNotNull(castResult.Data);
        }

        [TestMethod]
        public async Task UpdateShipPosition_Should_Invoke_The_Correct_Methods_And_Return_Failure_For_Get_Game()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Message = "error", Success = false });

            var result = await _shipController.UpdateShipPosition(new UpdateShipPositionRequest { GameId = 1, PlayerId = 1, ShipOrientationType = ShipOrientationType.HorizontalRight, ShipType = ShipType.Cruiser, StartPosition = new CoordinatesViewModel(1, 1) });

            var castResult = ValidateOkResult<Result<ShipViewModel>>(result);
            Assert.IsFalse(castResult.Success);
            Assert.IsNotNull(castResult.Message);
        }

        [TestMethod]
        public async Task UpdateShipPosition_Should_Invoke_The_Correct_Methods_And_Return_Failure_For_Get_Update_Position()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(new GameViewModel { Ships = new List<ShipViewModel>() });
            _shipUpdateService.Setup(s => s.UpdateShipPosition(It.IsAny<List<ShipViewModel>>(), It.IsAny<CoordinatesViewModel>(), It.IsAny<ShipType>(), It.IsAny<ShipOrientationType>())).Returns(new Result { Success = false, Message = "error" });

            var result = await _shipController.UpdateShipPosition(new UpdateShipPositionRequest { GameId = 1, PlayerId = 1, ShipOrientationType = ShipOrientationType.HorizontalRight, ShipType = ShipType.Cruiser, StartPosition = new CoordinatesViewModel(1, 1) });

            var castResult = ValidateOkResult<Result<ShipViewModel>>(result);
            Assert.IsFalse(castResult.Success);
            Assert.IsNotNull(castResult.Message);
        }

        [TestMethod]
        public async Task UpdateShipPosition_Should_Invoke_The_Correct_Methods_And_Return_Failure_For_Save_Changes()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(new GameViewModel { Ships = new List<ShipViewModel>() });
            _shipUpdateService.Setup(s => s.UpdateShipPosition(It.IsAny<List<ShipViewModel>>(), It.IsAny<CoordinatesViewModel>(), It.IsAny<ShipType>(), It.IsAny<ShipOrientationType>())).Returns(new Result { Success = true});
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = false, Message = "error" });

            var result = await _shipController.UpdateShipPosition(new UpdateShipPositionRequest { GameId = 1, PlayerId = 1, ShipOrientationType = ShipOrientationType.HorizontalRight, ShipType = ShipType.Cruiser, StartPosition = new CoordinatesViewModel(1, 1) });

            var castResult = ValidateOkResult<Result<ShipViewModel>>(result);
            Assert.IsFalse(castResult.Success);
            Assert.IsNotNull(castResult.Message);
        }
    }
}
