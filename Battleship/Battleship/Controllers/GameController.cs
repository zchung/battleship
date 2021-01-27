
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
        private readonly IGameValidationService _gameValidationService;

        public GameController(IGameDbService gameDbService, IGameFactory gameFactory, 
                              IGameUpdateService gameUpdateService, IHubContext<GameHub, IGameHub> gameHubContext,
                              IGameValidationService gameValidationService)
        {
            _gameDbService = gameDbService;
            _gameFactory = gameFactory;
            _gameUpdateService = gameUpdateService;
            _gameHubContext = gameHubContext;
            _gameValidationService = gameValidationService;
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
            var joinValidationResult = _gameValidationService.CanJoinGame(joinGameRequest.GameId);
            if (!joinValidationResult.Success)
            {
                return Ok(joinValidationResult);
            }
            var updateGameResult =  await _gameUpdateService.UpdateGameAfterPlayerJoins(joinGameRequest.GameId);
            Result<GameViewModel> result = new Result<GameViewModel>();
            if (updateGameResult.Success)
            {
                result.Data = _gameFactory.GetGameViewModel(updateGameResult.Data, 2);
                result.Success = updateGameResult.Success;
                await _gameHubContext.Clients.All.RemoveGame(_gameFactory.GetGameListViewModel(updateGameResult.Data));
                await _gameHubContext.Clients.All.SendPlayerHasJoined(new UpdatedPlayer(result.Data.GameId, result.Data.PlayerId, updateGameResult.Data.BothPlayersReady));
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
        [HttpPost]
        [Route("setPlayerToReady")]
        public async Task<IActionResult> SetPlayerToReady(GamePlayerRequest gamePlayerRequest)
        {
            var result = await _gameUpdateService.UpdatePlayerToReady(gamePlayerRequest.GameId, gamePlayerRequest.PlayerId);
            if (result.Success)
            {
                await _gameHubContext.Clients.All.SendPlayerIsReady(new UpdatedPlayer(gamePlayerRequest.GameId, gamePlayerRequest.PlayerId, result.Data.BothPlayersReady));
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("setGameStatus")]
        public async Task<IActionResult> SetGameStatus(SetGameStatusRequest request)
        {
            var result = await _gameUpdateService.UpdateGameStatus(request.GameId, request.GameStatus);

            if (result.Success)
            {
                await _gameHubContext.Clients.All.SendUpdateGameStatus(new UpdatedGame(request.GameId, result.Data.CurrentPlayerIdTurn));
            }

            return Ok(result);
        }
        [HttpPost]
        [Route("attackPlayer")]
        public async Task<IActionResult> AttackPlayer(AttackPlayerRequest request)
        {
            var validationResult = _gameValidationService.CanAttackPlayer(request.GameId, request.PlayerIdAttacking);
            if (!validationResult.Success)
            {
                return Ok(validationResult);
            }

            var result = await _gameUpdateService.ResolvePlayerAttack(request.GameId, request.PlayerIdAttacking, 
                                                                      request.PlayerIdToAttack, request.CoordinatesViewModel);

            if (result.Success)
            {
                await _gameHubContext.Clients.All.SendAttackPlayerCoordinates(
                    result
                    );
            }


            return Ok(new Result<CoordinatesViewModel> { Success = result.Success, Data = result.Data });
        }
    }
}
