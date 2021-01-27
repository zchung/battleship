

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
    }
}
