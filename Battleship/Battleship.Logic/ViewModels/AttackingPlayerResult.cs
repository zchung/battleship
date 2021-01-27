using Battleship.Data.Models;

namespace Battleship.Logic.ViewModels
{
    public class AttackingPlayerResult : Result<CoordinatesViewModel>
    {
        public int GameId { get; set; }
        public int PlayerIdAttacking { get; set; }
        public int PlayerIdToAttack { get; set; }
        public int? WinnerOfGamePlayerId { get; set; }
        public AttackingPlayerResult(int gameId, int playerIdAttacking, int playerIdToAttack, 
            CoordinatesViewModel coordinatesViewModel, bool success, string message)
        {
            GameId = gameId;
            PlayerIdAttacking = playerIdAttacking;
            PlayerIdToAttack = playerIdToAttack;
            Data = coordinatesViewModel;
            Success = success;
            Message = message;
        }
    }
}
