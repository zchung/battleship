
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Enums;
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
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase, IGameController
    {
        private readonly IGameDbService _gameDbService;
        private readonly IGameFactory _gameFactory;
        private readonly IGameUpdateService _gameUpdateService;
        private readonly IHubContext<GameHub, IGameHub> _gameHubContext;

        public GameController(IGameDbService gameDbService, IGameFactory gameFactory, 
                              IGameUpdateService gameUpdateService, IHubContext<GameHub, IGameHub> gameHubContext)
        {
            _gameDbService = gameDbService;
            _gameFactory = gameFactory;
            _gameUpdateService = gameUpdateService;
            _gameHubContext = gameHubContext;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CreateGameRequest createGameRequest)
        {
            Game game = _gameFactory.CreateNewGame(createGameRequest.Description);
            var gameCreateResult = await _gameDbService.Create(game);
            Result<GameViewModel> result = new Result<GameViewModel>();
            if (gameCreateResult.Success)
            {
                result.Success = gameCreateResult.Success;
                result.Data = _gameFactory.GetGameViewModel(gameCreateResult.Data, 1);
                var gameListModel = _gameFactory.GetGameListViewModel(gameCreateResult.Data);
                await _gameHubContext.Clients.All.SendNewGame(gameListModel);
            }
            else
            {
                result.Message = gameCreateResult.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("getactive")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _gameDbService.GetActiveGames();

            return Ok(result);
        }

        [HttpPost]
        [Route("join")]
        public async Task<IActionResult> Join(JoinGameRequest joinGameRequest)
        {
            var updateGameResult =  await _gameUpdateService.UpdateGameAfterPlayerJoins(joinGameRequest.GameId);
            Result<GameViewModel> result = new Result<GameViewModel>();
            if (updateGameResult.Success)
            {
                result.Data = _gameFactory.GetGameViewModel(updateGameResult.Data, 2);
                result.Success = updateGameResult.Success;
                await _gameHubContext.Clients.All.RemoveGame(_gameFactory.GetGameListViewModel(updateGameResult.Data));
                await _gameHubContext.Clients.All.SendPlayerHasJoined(new JoinedPlayer(result.Data.GameId, result.Data.PlayerId));
            }
            else
            {
                result.Message = updateGameResult.Message;
            }
            return Ok(result);
        }

        [HttpGet]
        [Route("get/{gameId}/{playerId}")]
        public IActionResult Get(int gameId, int playerId)
        {

            Result<GameViewModel> result = new Result<GameViewModel>();
            var gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Success)
            {
                result.Success = true;
                result.Data = _gameFactory.GetGameViewModel(gameResult.Data, playerId);
            }
            else
            {
                result.Message = gameResult.Message;
            }

            return Ok(result);
        }
    }
}
