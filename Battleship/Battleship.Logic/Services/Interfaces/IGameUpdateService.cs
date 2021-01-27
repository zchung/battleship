
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.ViewModels;
using System.Threading.Tasks;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IGameUpdateService
    {
        Task<Result<Game>> UpdateGameAfterPlayerJoins(int gameId);
        Task<Result<Game>> UpdatePlayerToReady(int gameId, int playerId);
        Task<Result<Game>> UpdateGameStatus(int gameId, GameStatus status);
        Task<AttackingPlayerResult> ResolvePlayerAttack(int gameId, int playerIdAttacking, int playerToAttackId, CoordinatesViewModel coordinatesViewModel);
    }
}
