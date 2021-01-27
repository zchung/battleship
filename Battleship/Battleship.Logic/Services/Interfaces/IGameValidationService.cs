using Battleship.Data.Models;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IGameValidationService
    {
        Result CanAttackPlayer(int gameId, int playerIdAttacking);
    }
}
