
using Battleship.Logic.Enums;
using System.Collections.Generic;

namespace Battleship.Logic.ViewModels
{
    public class ShipViewModel
    {
        public string Name { get; set; }
        public ShipType ShipType { get; set; }
        public int Size { get; set; }
        public List<CoordinatesViewModel> Coordinates { get; set; }
        public ShipViewModel()
        {
            Coordinates = new List<CoordinatesViewModel>();
        }
    }
}
