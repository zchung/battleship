
using Battleship.Logic.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Battleship.Tests.Factories
{
    [TestClass]
    public class CoordinatesFactoryTests
    {
        private ICoordinatesFactory _coordinatesFactory;

        [TestInitialize]
        public void Initialise()
        {
            _coordinatesFactory = new CoordinatesFactory();
        }
        [TestMethod]
        public void GetNewCoordinates_Within_The_Boundaries_Should_Return_The_Correct_Coordinates()
        {
            int size = 3;
            int size2 = 5;
            var startCoordinates = new CoordinatesViewModel(1, 1);
            var result = _coordinatesFactory.GenerateNewCoordinates(startCoordinates, size, ShipOrientationType.HorizontalRight);

            Assert.AreEqual(size, result.Count);
            for (int i = 0; i < result.OrderBy(x => x.XPosition).ToList().Count; i++)
            {
                Assert.AreEqual(i + 1, result[i].XPosition);
            }

            result = _coordinatesFactory.GenerateNewCoordinates(startCoordinates, size2, ShipOrientationType.VerticalDown);

            Assert.AreEqual(size2, result.Count);
            for (int i = 0; i < result.OrderBy(x => x.YPosition).ToList().Count; i++)
            {
                Assert.AreEqual(i + 1, result[i].YPosition);
            }
        }
    }
}
