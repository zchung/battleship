
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels;
using Battleship.Logic.InternalModels.Interfaces;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.Tests.Services
{
    [TestClass]
    public class GameUpdateServiceTests
    {
        private Mock<IGameDbService> _gameDbService;
        private Mock<IGameFactory> _gameFactory;
        private IPlayerFactory _playerFactory;
        private GameUpdateService _gameUpdateService;

        [TestInitialize]
        public void Initialise()
        {
            _gameDbService = new Mock<IGameDbService>();
            _gameFactory = new Mock<IGameFactory>();
            _playerFactory = new PlayerFactory();
            _gameUpdateService = new GameUpdateService(_gameDbService.Object, _gameFactory.Object, _playerFactory);
        }
        [TestMethod]
        public async Task UpdateGameAfterPlayerJoins_Should_Correctly_Update_The_Game()
        {
            var game = new Game();
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = game, Success = true });
            _gameFactory.Setup(s => s.GetGameType(It.IsAny<Game>(), It.IsAny<GameStatus>())).Returns(new PlanningGame(game));
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdateGameAfterPlayerJoins(1);

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(GameStatus.Planning, result.Data.GameStatus);
            Assert.AreEqual(PlayerStatus.Preparing, result.Data.Player2Status);
        }

        [TestMethod]
        public async Task UpdateGameAfterPlayerJoins_Should_Return_An_Error_When_Failing_To_Get_Game()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> {Success = false });

            var result = await _gameUpdateService.UpdateGameAfterPlayerJoins(1);

            Assert.IsFalse(result.Success);
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public async Task UpdateGameAfterPlayerJoins_Should_Return_An_Error_When_Failing_To_Save_Game()
        {
            var game = new Game();
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = game, Success = true });
            _gameFactory.Setup(s => s.GetGameType(It.IsAny<Game>(), It.IsAny<GameStatus>())).Returns(new PlanningGame(game));
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = false });

            var result = await _gameUpdateService.UpdateGameAfterPlayerJoins(1);

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);

            Assert.IsFalse(result.Success);
        }

        [TestMethod]
        public async Task UpdatePlayer1ToReady_Should_Update_The_Status_If_The_Condition_Is_Met()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel 
            { 
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        { 
                            new CoordinatesViewModel(1, 1),
                            new CoordinatesViewModel(1, 2)
                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 1);

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);
            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task UpdatePlayer2ToReady_Should_Update_The_Status_If_The_Condition_Is_Met()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 2)).Returns(new GameViewModel
            {
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        {
                            new CoordinatesViewModel(1, 1),
                            new CoordinatesViewModel(1, 2)
                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 2);

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);
            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task UpdatePlayerToPrepared_Should_Not_Update_The_Status_If_The_Condition_Is_Not_Met()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel
            {
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        {

                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 1);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Never);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public async Task UpdatePlayerToPrepared_Should_Return_Error_If_Save_Failed()
        {
            _gameDbService.Setup(s => s.GetById(1)).Returns(new Result<Game> { Data = new Game(), Success = true });
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(new GameViewModel
            {
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Size = 2,
                        Coordinates = new List<CoordinatesViewModel>
                        {

                        }
                    }
                }
            });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = false, Message = "Error" });

            var result = await _gameUpdateService.UpdatePlayerToReady(1, 1);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Never);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public async Task UpdateGameStatus_Should_Correctly_Update_The_Status()
        {
            Game game = new Game();
            GameStatus gameStatus = GameStatus.Started;
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });
            _gameFactory.Setup(s => s.GetGameType(It.IsAny<Game>(), It.IsAny<GameStatus>())).Returns(new Mock<IGameType>().Object);

            var result = await _gameUpdateService.UpdateGameStatus(1, gameStatus);

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);
            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.AreEqual(gameStatus, game.GameStatus);
        }

        [TestMethod]
        public async Task UpdateGameStatus_Should_Return_False_If_Save_Fails()
        {
            Game game = new Game();
            GameStatus gameStatus = GameStatus.Started;
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = false, Message = "Error" });
            _gameFactory.Setup(s => s.GetGameType(It.IsAny<Game>(), It.IsAny<GameStatus>())).Returns(new Mock<IGameType>().Object);

            var result = await _gameUpdateService.UpdateGameStatus(1, gameStatus);

            _gameDbService.Verify(v => v.SaveChangesAsync(), Times.Once);

            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Message);
        }

        [TestMethod]
        public async Task ResolvePlayerAttack_Should_Show_Correct_Result_When_Hitting_Defending_Player()
        {
            var game = new Game { CurrentPlayerIdTurn = 1 };
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            var gameViewModel = new GameViewModel
            {
                AttemptedCoordinates = new List<CoordinatesViewModel>(),
                Ships = new List<ShipViewModel> 
                { 
                    new ShipViewModel
                    {
                        Coordinates = new List<CoordinatesViewModel>
                        {
                            new CoordinatesViewModel(1, 1),
                            new CoordinatesViewModel(1, 2)
                        }
                    }
                }
            };
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(gameViewModel);
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });
            
            var result = await _gameUpdateService.ResolvePlayerAttack(1, 1, 2, new CoordinatesViewModel(1, 1));

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(gameViewModel.AttemptedCoordinates.Count > 0);
            Assert.IsTrue(gameViewModel.Ships.FirstOrDefault().Coordinates.FirstOrDefault().Hit.Value);
            Assert.AreEqual($"Player 1 Hits Player 2", result.Message);
            Assert.IsNotNull(game.Player1AttemptedCoordinatesJSON);
            Assert.IsNotNull(game.Player2ShipsJSON);
            Assert.AreNotEqual(1, game.CurrentPlayerIdTurn);
            Assert.IsNull(result.WinnerOfGamePlayerId);
        }

        [TestMethod]
        public async Task ResolvePlayerAttack_Should_Show_Correct_Result_When_Hitting_Defending_Player_And_Defeating_Them()
        {
            var game = new Game { CurrentPlayerIdTurn = 1 };
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            var gameViewModel = new GameViewModel
            {
                AttemptedCoordinates = new List<CoordinatesViewModel>(),
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Coordinates = new List<CoordinatesViewModel>
                        {
                            new CoordinatesViewModel(1, 1)
                        }
                    }
                }
            };
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(gameViewModel);
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.ResolvePlayerAttack(1, 1, 2, new CoordinatesViewModel(1, 1));

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(gameViewModel.AttemptedCoordinates.Count > 0);
            Assert.IsTrue(gameViewModel.Ships.FirstOrDefault().Coordinates.FirstOrDefault().Hit.Value);
            Assert.AreEqual($"Player 1 Hits Player 2", result.Message);
            Assert.IsNotNull(game.Player1AttemptedCoordinatesJSON);
            Assert.IsNotNull(game.Player2ShipsJSON);
            Assert.AreNotEqual(1, game.CurrentPlayerIdTurn);
            Assert.AreEqual(1, result.WinnerOfGamePlayerId);
            Assert.AreEqual(GameStatus.Completed, game.GameStatus);
        }

        [TestMethod]
        public async Task ResolvePlayerAttack_Should_Show_Correct_Result_When_Attempting_Already_Tried_Coordinates()
        {
            var game = new Game { CurrentPlayerIdTurn = 1 };
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            var gameViewModel = new GameViewModel
            {
                AttemptedCoordinates = new List<CoordinatesViewModel>
                {
                    new CoordinatesViewModel(1, 1)
                },
                
            };
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), It.IsAny<int>())).Returns(gameViewModel);

            var result = await _gameUpdateService.ResolvePlayerAttack(1, 1, 2, new CoordinatesViewModel(1, 1));

            Assert.IsFalse(result.Success);
            Assert.AreEqual("This coordinate has already been selected", result.Message);
            Assert.IsNull(game.Player1AttemptedCoordinatesJSON);
            Assert.IsNull(game.Player2ShipsJSON);
            Assert.AreEqual(1, game.CurrentPlayerIdTurn);
        }

        [TestMethod]
        public async Task ResolvePlayerAttack_Should_Show_Correct_Result_When_Missing_Defending_Player()
        {
            var game = new Game();
            _gameDbService.Setup(s => s.GetById(It.IsAny<int>())).Returns(new Result<Game> { Data = game, Success = true });
            var gameViewModelPlayer1 = new GameViewModel
            {
                AttemptedCoordinates = new List<CoordinatesViewModel>(),
                
            };
            var gameViewModelPlayer2 = new GameViewModel
            {
                Ships = new List<ShipViewModel>
                {
                    new ShipViewModel
                    {
                        Coordinates = new List<CoordinatesViewModel>
                        {
                            new CoordinatesViewModel(1, 2)
                        }
                    }
                }
            };
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 1)).Returns(gameViewModelPlayer1);
            _gameFactory.Setup(s => s.GetGameViewModel(It.IsAny<Game>(), 2)).Returns(gameViewModelPlayer2);
            _gameDbService.Setup(s => s.SaveChangesAsync()).ReturnsAsync(new Result { Success = true });

            var result = await _gameUpdateService.ResolvePlayerAttack(1, 1, 2, new CoordinatesViewModel(1, 1));

            _gameDbService.Verify(v => v.Update(It.IsAny<Game>()), Times.Once);

            Assert.IsTrue(result.Success);
            Assert.IsTrue(gameViewModelPlayer1.AttemptedCoordinates.Count > 0);
            Assert.AreEqual($"Player 1 Misses Player 2", result.Message);
            Assert.IsNotNull(game.Player1AttemptedCoordinatesJSON);
            Assert.IsNotNull(game.Player2ShipsJSON);
            Assert.AreNotEqual(1, game.CurrentPlayerIdTurn);
        }
    }
}
