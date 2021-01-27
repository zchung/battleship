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
using Battleship.Data.Enums;

namespace Battleship.Tests.Controllers
{    
    [TestClass]
    public class GameControllerTests : ControllerTestBase
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IGameFactory> _gameFactory;
        private Mock<IGameUpdateService> _gameUpdateService;
        private Mock<IHubContext<GameHub, IGameHub>> _gameHubContext;
        private Mock<IGameValidationService> _gameValidationService;
        private IGameController _gameController;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameFactory = new Mock<IGameFactory>();
            _gameUpdateService = new Mock<IGameUpdateService>();            
            _gameHubContext = new Mock<IHubContext<GameHub, IGameHub>>();
            var gameHub = new Mock<IGameHub>();
            _gameHubContext.Setup(s => s.Clients.All).Returns(gameHub.Object);
            _gameValidationService = new Mock<IGameValidationService>();
            _gameController = new GameController(_gameDbService.Object, _gameFactory.Object, _gameUpdateService.Object, _gameHubContext.Object, _gameValidationService.Object);
        }

        [TestMethod]
        public async Task Create_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.Create(It.IsAny<Game>())).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel());
            _gameFactory.Setup(s => s.GetGameListViewModel(It.IsAny<Game>())).Returns(new List());
            _gameFactory.Setup(s => s.CreateNewGame(It.IsAny<string>())).Returns(new Game());

            var result = await _gameController.Create(new CreateGameRequest { Description = "Test Game"});

            _gameHubContext.Verify(v => v.Clients.All.SendNewGame(It.IsAny<List>()), Times.Once);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task GetActive_Should_Return_The_Result()
        {
            _gameDbService.Setup(s => s.GetActiveGames()).ReturnsAsync(new Result<IEnumerable<List>> { Success = true });

            var result = await _gameController.GetActive();

            var castresult = ValidateOkResult<Result<IEnumerable<List>>>(result);
            Assert.IsTrue(castresult.Success);
        }

        [TestMethod]
        public async Task Join_Should_Correctly_Return_The_Updated_Result()
        {
            _gameUpdateService.Setup(s => s.UpdateGameAfterPlayerJoins(1)).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 2)).Returns(new GameViewModel());
            _gameFactory.Setup(s => s.GetGameListViewModel(It.IsAny<Game>())).Returns(new List());

            var result = await _gameController.Join(new JoinGameRequest { GameId = 1 });

            _gameHubContext.Verify(v => v.Clients.All.RemoveGame(It.IsAny<List>()), Times.Once);
            _gameHubContext.Verify(v => v.Clients.All.SendPlayerHasJoined(It.IsAny<UpdatedPlayer>()), Times.Once);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public void Get_Should_Return_The_Game_Data_If_Found()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(new GameViewModel());

            var result = _gameController.Get(1, 1);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsTrue(castResult.Success);
            Assert.IsNotNull(castResult.Data);
        }

        [TestMethod]
        public void Get_Should_Return_The_Result_If_Game_Not_Found()
        {
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Success = false, Message = "error" });

            var result = _gameController.Get(1, 1);

            var castResult = ValidateOkResult<Result<GameViewModel>>(result);
            Assert.IsFalse(castResult.Success);
            Assert.IsNotNull(castResult.Message);
        }

        [TestMethod]
        public async Task SetGameToPrepared_Should_Return_The_Result()
        {
            _gameUpdateService.Setup(s => s.UpdatePlayerToReady(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true});

            var result = await _gameController.SetPlayerToReady(new GamePlayerRequest { GameId = 1, PlayerId = 1 });

            _gameHubContext.Verify(v => v.Clients.All.SendPlayerIsReady(It.IsAny<UpdatedPlayer>()), Times.Once);

            var castResult = ValidateOkResult<Result>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task SetGameStatus_Should_Return_The_Result()
        {
            _gameUpdateService.Setup(s => s.UpdateGameStatus(It.IsAny<int>(), It.IsAny<GameStatus>())).ReturnsAsync(new Result<Game> { Data = new Game(), Success = true });

            var result = await _gameController.SetGameStatus(new SetGameStatusRequest { GameId = 1, GameStatus = GameStatus.Started });

            var castResult = ValidateOkResult<Result>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task AttackPlayer_Should_Correctly_Invoke_The_Right_Methods()
        {
            _gameValidationService.Setup(s => s.CanAttackPlayer(It.IsAny<int>(), It.IsAny<int>())).Returns(new Result { Success = true });
            _gameUpdateService.Setup(s => s.ResolvePlayerAttack(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CoordinatesViewModel>()))
                .ReturnsAsync(new Result<CoordinatesViewModel> { Success = true, Data = new CoordinatesViewModel(1, 1)});

            var result = await _gameController.AttackPlayer(new AttackPlayerRequest());

            var castResult = ValidateOkResult<Result<CoordinatesViewModel>>(result);
            Assert.IsTrue(castResult.Success);
        }

        [TestMethod]
        public async Task AttackPlayer_Should_Return_False_Result_If_Unable_To_Attack()
        {
            _gameValidationService.Setup(s => s.CanAttackPlayer(It.IsAny<int>(), It.IsAny<int>())).Returns(new Result { Success = false, Message = "unable to attack" });

            var result = await _gameController.AttackPlayer(new AttackPlayerRequest());

            var castResult = ValidateOkResult<Result>(result);
            Assert.IsFalse(castResult.Success);
        }
    }
}
