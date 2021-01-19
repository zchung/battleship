using Battleship.Controllers;
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Hubs;
using Battleship.Hubs.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.Tests.Controllers
{    
    [TestClass]
    public class GameControllerTests
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IHubContext<GameHub, IGameHub>> _gameHubContext;
        private IGameController _gameController;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameHubContext = new Mock<IHubContext<GameHub, IGameHub>>();
            _gameController = new GameController(_gameDbService.Object, _gameHubContext.Object);
        }

        [TestMethod]
        public async Task Create_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.Create(It.IsAny<Game>())).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });
            var gameHub = new Mock<IGameHub>();
            _gameHubContext.Setup(s => s.Clients.All).Returns(gameHub.Object);

            var result = await _gameController.Create(new CreateGameRequest());

            _gameHubContext.Verify(v => v.Clients.All.SendNewGame(It.IsAny<Game>()), Times.Once);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOfType(okObject.Value, typeof(Result<Game>));
            var castResult = okObject.Value as Result<Game>;
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task GetActive_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.GetActiveGames()).ReturnsAsync(new Result<IEnumerable<Game>> { Success = true });

            var result = await _gameController.GetActive();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObject = result as OkObjectResult;
            Assert.IsInstanceOfType(okObject.Value, typeof(Result<IEnumerable<Game>>));
            var castresult = okObject.Value as Result<IEnumerable<Game>>;
            Assert.IsTrue(castresult.Success);
        }
    }
}
