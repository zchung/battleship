
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels;
using Battleship.Logic.InternalModels.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests.InternalModels
{
    [TestClass]
    public class GameTypeTests
    {
        [TestMethod]
        public void HandleStatusChange_With_StartedGame_Should_Correctly_Update_The_Game()
        {
            var game = new Game();
            IGameType gameType = new StartedGame(game, new RandomGeneratorFactory());
            gameType.HandleStatusUpdate();

            Assert.IsNotNull(game.CurrentPlayerIdTurn);
        }

        [TestMethod]
        public void HandleStatusChange_With_PlanningGame_Should_Correctly_Update_The_Game()
        {
            var game = new Game();
            IGameType gameType = new PlanningGame(game);
            gameType.HandleStatusUpdate();

            Assert.AreEqual(GameStatus.Planning, game.GameStatus);
            Assert.AreEqual(PlayerStatus.Preparing, game.Player2Status);
        }
    }
}
