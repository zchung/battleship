

using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.Factories
{
    [TestClass]
    public class PlayerFactoryTests
    {
        private IPlayerFactory _playerFactory;

        [TestInitialize]
        public void Initialise()
        {
            _playerFactory = new PlayerFactory();
        }

        [TestMethod]
        public void GetPlayer_Should_Get_The_Correct_Player()
        {
            var player1 = _playerFactory.GetPlayer(1);

            Assert.IsInstanceOfType(player1, typeof(Player1));

            var player2 = _playerFactory.GetPlayer(2);

            Assert.IsInstanceOfType(player2, typeof(Player2));
        }
    }
}
