
using Battleship.Logic.Enums;
using System.Collections.Generic;
using System.Linq;

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

        public bool IsPlaced()
        {
            return Coordinates.Count == Size;
        }

        public bool IsDestroyed()
        {
            return Coordinates.TrueForAll(x => x.Hit);
        }
        public CoordinatesViewModel FindCoordinatesInShip(CoordinatesViewModel coordinatesViewModel)
        {
            return Coordinates.FirstOrDefault(x => x.XPosition == coordinatesViewModel.XPosition && x.YPosition == coordinatesViewModel.YPosition);
        }
    }
}
