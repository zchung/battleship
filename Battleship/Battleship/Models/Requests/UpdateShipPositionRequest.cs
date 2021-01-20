

using Battleship.Logic.Enums;
using Battleship.Logic.ViewModels;

namespace Battleship.Models.Requests
{
    public class UpdateShipPositionRequest
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public ShipType ShipType { get; set; }
        public CoordinatesViewModel StartPosition { get; set; }
        public ShipOrientationType ShipOrientationType { get; set; }
    }
}
