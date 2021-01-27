
using Battleship.Logic.ViewModels;

namespace Battleship.Models.Requests
{
    public class AttackPlayerRequest
    {
        public int GameId { get; set; }
        public int PlayerIdAttacking { get; set; }
        public int PlayerIdToAttack { get; set; }
        public CoordinatesViewModel CoordinatesViewModel { get; set; }
    }
}
