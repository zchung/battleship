

using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;

namespace Battleship.Logic.Services
{
    public class GameValidationService : IGameValidationService
    {
        private readonly IGameDbService _gameDbService;
        public GameValidationService(IGameDbService gameDbService)
        {
            _gameDbService = gameDbService;
        }
        public Result CanAttackPlayer(int gameId, int playerIdAttacking)
        {
            Result result = new Result();
            var gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Success)
            {
                if (gameResult.Data.GameStatus == GameStatus.Completed)
                {
                    result.Message = "This game is completed. You cannot play";
                    return result;
                }
                if (gameResult.Data.CurrentPlayerIdTurn == playerIdAttacking)
                {
                    result.Success = true;
                }
                else
                {
                    result.Message = "It is not your turn.";
                }
            }
            else
            {
                result.Message = gameResult.Message;
            }

            return result;
        }

        public Result CanJoinGame(int gameId)
        {
            Result result = new Result();
            var gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Success)
            {
                result.Success = true;
                if (gameResult.Data.GameStatus != GameStatus.Active)
                {
                    result.Success = false;
                    result.Message = "Cannot join game, it has already started";
                }
            }
            else
            {
                result.Message = gameResult.Message;
            }
            return result;
        }
    }
}
