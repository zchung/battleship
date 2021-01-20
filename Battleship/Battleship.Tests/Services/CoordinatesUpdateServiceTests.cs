
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class CoordinatesUpdateServiceTests
    {
        private Mock<ICoordinatesFactory> _coordinatesFactory;
        private Mock<ICoordinatesValidationService> _coordinatesValidationService;
        private ICoordinatesUpdateService _coordinatesUpdateService;

        [TestInitialize]
        public void Initialise()
        {
            _coordinatesFactory = new Mock<ICoordinatesFactory>();
            _coordinatesValidationService = new Mock<ICoordinatesValidationService>();
            _coordinatesUpdateService = new CoordinatesUpdateService(_coordinatesFactory.Object, _coordinatesValidationService.Object);
        }

        [TestMethod]
        public void UpdateNewCoordinates_Should_Invoke_The_Correct_Methods_And_Return_Success_Results()
        {
            _coordinatesFactory.Setup(s => s.GenerateNewCoordinates(It.IsAny<CoordinatesViewModel>(), 1, It.IsAny<ShipOrientationType>())).Returns(new List<CoordinatesViewModel>());
            _coordinatesValidationService.Setup(s => s.ValidateCoordinatesInGameSpace(It.IsAny<List<CoordinatesViewModel>>())).Returns(new Result { Success = true });
            _coordinatesValidationService.Setup(s => s.ValidateCoordinatesForOverlapping(It.IsAny<List<CoordinatesViewModel>>(), It.IsAny<List<CoordinatesViewModel>>())).Returns(new Result { Success = true });

            var result = _coordinatesUpdateService.UpdateNewCoordinates(new CoordinatesViewModel(), 1, ShipOrientationType.HorizontalRight, new List<CoordinatesViewModel>());

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public void UpdateNewCoordinates_Should_Invoke_The_Correct_Methods_And_Return_Fail_GameboardBound_Results()
        {
            _coordinatesFactory.Setup(s => s.GenerateNewCoordinates(It.IsAny<CoordinatesViewModel>(), 1, It.IsAny<ShipOrientationType>())).Returns(new List<CoordinatesViewModel>());
            _coordinatesValidationService.Setup(s => s.ValidateCoordinatesInGameSpace(It.IsAny<List<CoordinatesViewModel>>())).Returns(new Result { Success = false, Message = "error" });

            var result = _coordinatesUpdateService.UpdateNewCoordinates(new CoordinatesViewModel(), 1, ShipOrientationType.HorizontalRight, new List<CoordinatesViewModel>());

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public void UpdateNewCoordinates_Should_Invoke_The_Correct_Methods_And_Return_Fail_Overlap_Results()
        {
            _coordinatesFactory.Setup(s => s.GenerateNewCoordinates(It.IsAny<CoordinatesViewModel>(), 1, It.IsAny<ShipOrientationType>())).Returns(new List<CoordinatesViewModel>());
            _coordinatesValidationService.Setup(s => s.ValidateCoordinatesInGameSpace(It.IsAny<List<CoordinatesViewModel>>())).Returns(new Result { Success = true });
            _coordinatesValidationService.Setup(s => s.ValidateCoordinatesForOverlapping(It.IsAny<List<CoordinatesViewModel>>(), It.IsAny<List<CoordinatesViewModel>>())).Returns(new Result { Success = false, Message = "error" });

            var result = _coordinatesUpdateService.UpdateNewCoordinates(new CoordinatesViewModel(), 1, ShipOrientationType.HorizontalRight, new List<CoordinatesViewModel>());

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }
    }
}
