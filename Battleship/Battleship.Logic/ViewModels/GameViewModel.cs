
using Battleship.Data.Enums;
using System.Collections.Generic;

namespace Battleship.Logic.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Description { get; set; }
        public int PlayerId { get; set; }

        public List<ShipViewModel> Ships { get; set; }
        public PlayerStatus Player1Status { get; set; }
        public PlayerStatus Player2Status { get; set; }
        public bool AllShipsPlaced()
        {
            return Ships.TrueForAll(s => s.IsPlaced());
        }
    }
}
