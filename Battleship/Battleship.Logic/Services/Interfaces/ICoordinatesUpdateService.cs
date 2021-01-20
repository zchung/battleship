
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Services.Interfaces
{
    public interface ICoordinatesUpdateService
    {
        Result<List<CoordinatesViewModel>> UpdateNewCoordinates(CoordinatesViewModel startPosition, int size, ShipOrientationType shipOrientationType, List<CoordinatesViewModel> existingCoordintes);
    }
}
