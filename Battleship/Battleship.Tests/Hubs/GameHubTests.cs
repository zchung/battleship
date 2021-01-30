using Battleship.Hubs;
using Battleship.Hubs.Interfaces;
using Battleship.Logic.ViewModels;
using Battleship.Models.Hub;
using Microsoft.AspNetCore.SignalR;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Tests.Hubs
{
    [TestClass]
    public class GameHubTests
    {
        private Mock<IHubCallerClients<IGameHub>> _hubCallerClients;
        private GameHub _gameHub;

        [TestInitialize]
        public void Initialise()
        {
            _hubCallerClients = new Mock<IHubCallerClients<IGameHub>>();
            _gameHub = new GameHub();
            _gameHub.Clients = _hubCallerClients.Object;
        }

        [TestMethod]
        public async Task SendNewGame_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendNewGame(It.IsAny<GameListViewModel>()));
            await _gameHub.SendNewGame(new GameListViewModel { GameId = 1, Description = "New Game" });

            _hubCallerClients.Verify(v => v.All.SendNewGame(It.IsAny<GameListViewModel>()), Times.Once);
        }

        [TestMethod]
        public async Task RemoveGame_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.RemoveGame(It.IsAny<GameListViewModel>()));
            await _gameHub.RemoveGame(new GameListViewModel { GameId = 1, Description = "New Game" });

            _hubCallerClients.Verify(v => v.All.RemoveGame(It.IsAny<GameListViewModel>()), Times.Once);
        }

        [TestMethod]
        public async Task SendPlayerHasJoined_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendPlayerHasJoined(It.IsAny<UpdatedPlayer>()));
            await _gameHub.SendPlayerHasJoined(new UpdatedPlayer(1, 1, false));

            _hubCallerClients.Verify(v => v.All.SendPlayerHasJoined(It.IsAny<UpdatedPlayer>()), Times.Once);
        }

        [TestMethod]
        public async Task SendPlayerIdReady_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendPlayerIsReady(It.IsAny<UpdatedPlayer>()));
            await _gameHub.SendPlayerIsReady(new UpdatedPlayer(1, 1, false));

            _hubCallerClients.Verify(v => v.All.SendPlayerIsReady(It.IsAny<UpdatedPlayer>()), Times.Once);
        }

        [TestMethod]
        public async Task SendBothPlayersReady_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendBothPlayersReady(It.IsAny<UpdatedGame>()));
            await _gameHub.SendBothPlayersReady(new UpdatedGame(1, 1));

            _hubCallerClients.Verify(v => v.All.SendBothPlayersReady(It.IsAny<UpdatedGame>()), Times.Once);
        }

        [TestMethod]
        public async Task SendUpdateGameStatus_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendUpdateGameStatus(It.IsAny<UpdatedGame>()));
            await _gameHub.SendUpdateGameStatus(new UpdatedGame(1, 1));

            _hubCallerClients.Verify(v => v.All.SendUpdateGameStatus(It.IsAny<UpdatedGame>()), Times.Once);
        }

        [TestMethod]
        public async Task SendAttackPlayerCoordinates_Should_Correctly_Invoke_The_Hub()
        {
            _hubCallerClients.Setup(s => s.All.SendAttackPlayerCoordinates(It.IsAny<AttackingPlayerResult>()));
            await _gameHub.SendAttackPlayerCoordinates(new AttackingPlayerResult(1, 1, 1, new CoordinatesViewModel(), true, null));

            _hubCallerClients.Verify(v => v.All.SendAttackPlayerCoordinates(It.IsAny<AttackingPlayerResult>()), Times.Once);
        }
    }
}
