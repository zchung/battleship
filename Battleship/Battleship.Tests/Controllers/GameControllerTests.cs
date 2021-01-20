using Battleship.Controllers;
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.ViewModels;
using Battleship.Hubs;
using Battleship.Hubs.Interfaces;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Models.Hub;
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Tests.Controllers
{    
    [TestClass]
    public class GameControllerTests
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IGameFactory> _gameFactory;
        private Mock<IGameUpdateService> _gameUpdateService;
        private Mock<IHubContext<GameHub, IGameHub>> _gameHubContext;
        private IGameController _gameController;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameFactory = new Mock<IGameFactory>();
            _gameUpdateService = new Mock<IGameUpdateService>();
            _gameHubContext = new Mock<IHubContext<GameHub, IGameHub>>();
            _gameController = new GameController(_gameDbService.Object, _gameFactory.Object, _gameUpdateService.Object, _gameHubContext.Object);
        }

        [TestMethod]
        public async Task Create_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.Create(It.IsAny<Game>())).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });
            var gameHub = new Mock<IGameHub>();
            _gameHubContext.Setup(s => s.Clients.All).Returns(gameHub.Object);
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel());
            _gameFactory.Setup(s => s.GetGameListViewModel(It.IsAny<Game>())).Returns(new GameListViewModel());
            _gameFactory.Setup(s => s.CreateNewGame(It.IsAny<string>())).Returns(new Game());

            var result = await _gameController.Create(new CreateGameRequest());

            _gameHubContext.Verify(v => v.Clients.All.SendNewGame(It.IsAny<GameListViewModel>()), Times.Once);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task GetActive_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.GetActiveGames()).ReturnsAsync(new Result<IEnumerable<GameListViewModel>> { Success = true });

            var result = await _gameController.GetActive();

            var castresult = ValidateOkResult<Result<IEnumerable<GameListViewModel>>>(result);
            Assert.IsTrue(castresult.Success);
        }

        [TestMethod]
        public async Task Join_Should_Correctly_Return_The_Updated_Result()
        {
            _gameUpdateService.Setup(s => s.UpdateGameAfterPlayerJoins(1)).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 2)).Returns(new GameViewModel());
            _gameFactory.Setup(s => s.GetGameListViewModel(It.IsAny<Game>())).Returns(new GameListViewModel());
            var gameHub = new Mock<IGameHub>();
            _gameHubContext.Setup(s => s.Clients.All).Returns(gameHub.Object);

            var result = await _gameController.Join(new JoinGameRequest { GameId = 1 });

            _gameHubContext.Verify(v => v.Clients.All.RemoveGame(It.IsAny<GameListViewModel>()), Times.Once);
            _gameHubContext.Verify(v => v.Clients.All.SendPlayerHasJoined(It.IsAny<JoinedPlayer>()), Times.Once);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsTrue(castResult.Success);
        }

        private T ValidateOkResult<T>(IActionResult result)
        {
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okObject =  result as OkObjectResult;
            Assert.IsInstanceOfType(okObject.Value, typeof(T));
            return (T)okObject.Value;
        }
    }
}
