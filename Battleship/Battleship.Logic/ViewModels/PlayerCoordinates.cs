

using System.Collections.Generic;

namespace Battleship.Logic.ViewModels
{
    public class PlayerCoordinates
    {
        public GameViewModel GameViewModel { get; set; }
        public List<CoordinatesViewModel> CoordinatesViewModels { get; set; }
        public bool AllShipsDestroyed { get; set; }
    }
}
