
using Battleship.Data.Enums;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.ViewModels
{
    [TestClass]
    public class GameViewModelTests
    {
        private GameViewModel _gameViewModel;

        [TestInitialize]
        public void Initialise()
        {
            _gameViewModel = new GameViewModel();
        }

        [TestMethod]
        public void GameReadyToStart_Should_Correctly_Handle_The_Scenarios()
        {
            _gameViewModel.Player1Status = PlayerStatus.Ready;
            _gameViewModel.Player2Status = PlayerStatus.Ready;

            bool result = _gameViewModel.GameReadyToStart;

            Assert.IsTrue(result);
        }
    }
}
