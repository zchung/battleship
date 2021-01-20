

using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface IShipFactory
    {
        List<ShipViewModel> GetDefaultShips();
    }
}
