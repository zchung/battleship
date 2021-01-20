
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.Factories
{
    [TestClass]
    public class ShipFactoryTests
    {
        private IShipFactory _shipFactory;

        [TestInitialize]
        public void Initialise()
        {
            _shipFactory = new ShipFactory();
        }

        [TestMethod]
        public void GetDefaultShips_Should_Return_Ships()
        {
            var result = _shipFactory.GetDefaultShips();

            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.Count);
        }
    }
}
