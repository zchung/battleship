using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;
using System.Threading.Tasks;

namespace Battleship.Logic.Services
{
    public class GameUpdateService : IGameUpdateService
    {
        private readonly IGameDbService _gameDbService;
        public GameUpdateService(IGameDbService gameDbService)
        {
            _gameDbService = gameDbService;
        }
        public async Task<Result<Game>> UpdateGameAfterPlayerJoins(int gameId)
        {
            Result<Game> gameResult = _gameDbService.GetById(gameId);

            if (gameResult.Data != null && gameResult.Success)
            {
                gameResult.Data.GameStatus = GameStatus.Started;
                await _gameDbService.SaveChangesAsync();
                gameResult.Success = true;
            }

            return gameResult;
        }
    }
}
