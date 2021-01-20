
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Factories
{
    public class CoordinatesFactory : ICoordinatesFactory
    {
        public List<CoordinatesViewModel> GenerateNewCoordinates(CoordinatesViewModel startPosition, int size, ShipOrientationType shipOrientationType)
        {
            List<CoordinatesViewModel> list = new List<CoordinatesViewModel> { startPosition };
            for (int i = 1; i < size; i++)
            {
                CoordinatesViewModel coordinatesViewModel = null;
                switch (shipOrientationType)
                {
                    case ShipOrientationType.HorizontalRight:
                        coordinatesViewModel = new CoordinatesViewModel(startPosition.XPosition + i, startPosition.YPosition);
                        break;
                    case ShipOrientationType.VerticalDown:
                        coordinatesViewModel = new CoordinatesViewModel(startPosition.XPosition, startPosition.YPosition + i);
                        break;
                    default:
                        break;
                }

                list.Add(coordinatesViewModel);
            }

            return list;
        }
    }
}
