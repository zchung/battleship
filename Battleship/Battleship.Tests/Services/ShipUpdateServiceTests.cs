
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class ShipUpdateServiceTests
    {
        private Mock<ICoordinatesUpdateService> _coordinatesUpdateService;
        private IShipUpdateService _shipUpdateService;

        [TestInitialize]
        public void Initialise()
        {
            _coordinatesUpdateService = new Mock<ICoordinatesUpdateService>();
            _shipUpdateService = new ShipUpdateService(_coordinatesUpdateService.Object);
        }
        [TestMethod]
        public void UpdateShipPosition_Should_Update_Ship_Position_If_Valid()
        {
            ShipFactory shipFactory = new ShipFactory();
            List<ShipViewModel> existingShips = shipFactory.GetDefaultShips();
            int y = 1;
            foreach (var item in existingShips)
            {              
                for (int i = 0; i < item.Size; i++)
                {
                    CoordinatesViewModel coordinatesViewModel = new CoordinatesViewModel(i + 1, y);
                    item.Coordinates.Add(coordinatesViewModel);
                }
                y++;
            }

            ShipType shipType = ShipType.Cruiser;

            _coordinatesUpdateService.Setup(s => s.UpdateNewCoordinates(It.IsAny<CoordinatesViewModel>(), It.IsAny<int>(), It.IsAny<ShipOrientationType>(), It.IsAny<List<CoordinatesViewModel>>())).Returns(
                new Result<List<CoordinatesViewModel>> { Data = new List<CoordinatesViewModel>(), Success = true });

            var result = _shipUpdateService.UpdateShipPosition(existingShips, new CoordinatesViewModel(1, 6), shipType, ShipOrientationType.HorizontalRight);

            Assert.IsTrue(result.Success);
            var updatedShip = existingShips.FirstOrDefault(x => x.ShipType == shipType);
            Assert.AreEqual(0, updatedShip.Coordinates.Count);
        }

        [TestMethod]
        public void UpdateShipPosition_Should_Not_Update_Position_If_Its_Not_Valid()
        {
            ShipFactory shipFactory = new ShipFactory();
            List<ShipViewModel> existingShips = shipFactory.GetDefaultShips();
            int y = 1;
            foreach (var item in existingShips)
            {
                for (int i = 0; i < item.Size; i++)
                {
                    CoordinatesViewModel coordinatesViewModel = new CoordinatesViewModel(i + 1, y);
                    item.Coordinates.Add(coordinatesViewModel);
                }
                y++;
            }

            ShipType shipType = ShipType.Cruiser;

            _coordinatesUpdateService.Setup(s => s.UpdateNewCoordinates(It.IsAny<CoordinatesViewModel>(), It.IsAny<int>(), It.IsAny<ShipOrientationType>(), It.IsAny<List<CoordinatesViewModel>>())).Returns(
                new Result<List<CoordinatesViewModel>> { Success = false, Message = "error" });

            var result = _shipUpdateService.UpdateShipPosition(existingShips, new CoordinatesViewModel(1, 6), shipType, ShipOrientationType.HorizontalRight);

            Assert.IsFalse(result.Success);
            var updatedShip = existingShips.FirstOrDefault(x => x.ShipType == shipType);
            Assert.AreEqual(3, updatedShip.Coordinates.Count);
        }
    }
}
