using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using System.Threading.Tasks;

namespace Battleship.Logic.Services
{
    public class GameUpdateService : IGameUpdateService
    {
        private readonly IGameDbService _gameDbService;
        private readonly IGameFactory _gameFactory;
        public GameUpdateService(IGameDbService gameDbService, IGameFactory gameFactory)
        {
            _gameDbService = gameDbService;
            _gameFactory = gameFactory;
        }
        public async Task<Result<Game>> UpdateGameAfterPlayerJoins(int gameId)
        {
            Result<Game> gameResult = _gameDbService.GetById(gameId);

            if (gameResult.Data != null && gameResult.Success)
            {
                gameResult.Data.GameStatus = GameStatus.Started;
                gameResult.Data.Player2Status = PlayerStatus.Preparing;
                await _gameDbService.SaveChangesAsync();
                gameResult.Success = true;
            }
            else
            {
                gameResult.Message = "Unable to update game after joining game.";
            }

            return gameResult;
        }

        public async Task<Result> UpdatePlayerToPrepared(int gameId, int playerId)
        {
            Result result = new Result();
            Result<Game> gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Data != null && gameResult.Success)
            {
                var gameViewModel = _gameFactory.GetGameViewModel(gameResult.Data, playerId);
                if (gameViewModel.AllShipsPlaced())
                {
                    if (playerId == 1)
                    {
                        gameResult.Data.Player1Status = PlayerStatus.Prepared;
                    }
                    else
                    {
                        gameResult.Data.Player2Status = PlayerStatus.Prepared;
                    }
                    var saveResult = await _gameDbService.SaveChangesAsync();
                    if (saveResult.Success)
                    {
                        result.Success = true;
                    }
                    else
                    {
                        result.Message = "Unable to update Status";
                    }
                }
                else
                {
                    result.Message = "You must place all ships before you are prepared.";
                }
            }
            else
            {
                gameResult.Message = "Unable to get game.";
            }

            return result;
        }
    }
}
