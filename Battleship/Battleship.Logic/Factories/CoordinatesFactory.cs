
using Battleship.Logic.Constants;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.ViewModels;
using System;
using System.Collections.Generic;

namespace Battleship.Logic.Factories
{
    public class CoordinatesFactory : ICoordinatesFactory
    {
        private readonly Random _random;
        public CoordinatesFactory()
        {
            _random = new Random();
        }

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

        public CoordinatesViewModel GetRandomCoordinate()
        {
            return new CoordinatesViewModel(_random.Next(1, CoordinatesConstants.MAX_X_COORDINATES + 1), _random.Next(1, CoordinatesConstants.MAX_Y_COORDINATES + 1));
        }

        public ShipOrientationType GetRandomOrientation()
        {
            return (ShipOrientationType)_random.Next((int)ShipOrientationType.HorizontalRight, (int)ShipOrientationType.VerticalDown + 1);
        }
        public List<CoordinatesViewModel> GenerateGameboard()
        {
            List<CoordinatesViewModel> grid = new List<CoordinatesViewModel>();
            for (int y = 1; y < CoordinatesConstants.MAX_Y_COORDINATES + 1; y++)
            {
                for (int x = 1; x < CoordinatesConstants.MAX_X_COORDINATES + 1; x++)
                {
                    grid.Add(new CoordinatesViewModel(x, y));

                }
            }
            return grid;
        }
    }
}
