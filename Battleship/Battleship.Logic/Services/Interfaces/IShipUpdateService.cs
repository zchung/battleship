
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IShipUpdateService
    {
        Result UpdateShipPosition(List<ShipViewModel> currentShipModels, CoordinatesViewModel startPosition, ShipType shipType, ShipOrientationType shipOrientationType);
    }
}
