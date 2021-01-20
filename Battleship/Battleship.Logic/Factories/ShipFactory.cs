
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Factories
{
    public class ShipFactory : IShipFactory
    {
        public List<ShipViewModel> GetDefaultShips()
        {
            return new List<ShipViewModel>
            {
                new ShipViewModel { ShipType = ShipType.Carrier, Name = ShipType.Carrier.ToString(), Size = 5 },
                new ShipViewModel { ShipType = ShipType.Battleship, Name = ShipType.Battleship.ToString(), Size = 4 },
                new ShipViewModel { ShipType = ShipType.Cruiser, Name = ShipType.Cruiser.ToString(), Size = 3 },
                new ShipViewModel { ShipType = ShipType.Submarine, Name = ShipType.Submarine.ToString(), Size = 3 },
                new ShipViewModel { ShipType = ShipType.Destroyer, Name = ShipType.Destroyer.ToString(), Size = 2 }
            };
        }
    }
}
