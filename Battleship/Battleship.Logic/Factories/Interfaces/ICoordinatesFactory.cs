
using Battleship.Logic.Enums;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface ICoordinatesFactory
    {
        List<CoordinatesViewModel> GenerateNewCoordinates(CoordinatesViewModel startPosition, int size, ShipOrientationType shipOrientationType);
    }
}
