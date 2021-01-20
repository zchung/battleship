using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Logic.Services
{
    public class ShipUpdateService : IShipUpdateService
    {
        private readonly ICoordinatesUpdateService _coordinatesUpdateService;

        public ShipUpdateService(ICoordinatesUpdateService coordinatesUpdateService)
        {
            _coordinatesUpdateService = coordinatesUpdateService;
        }
        public Result UpdateShipPosition(List<ShipViewModel> currentShipModels, CoordinatesViewModel startPosition, ShipType shipType, ShipOrientationType shipOrientationType)
        {
            Result result = new Result();
            ShipViewModel shipToUpdate = currentShipModels.FirstOrDefault(x => x.ShipType == shipType);
            List<CoordinatesViewModel> existingCoordinatesFromOtherShips = currentShipModels.Where(s => s.ShipType != shipType).Select(s => s.Coordinates).SelectMany(s => s).ToList();

            var coordinatesResult = _coordinatesUpdateService.UpdateNewCoordinates(startPosition, shipToUpdate.Size, shipOrientationType, existingCoordinatesFromOtherShips);

            if (coordinatesResult.Success)
            {
                shipToUpdate.Coordinates = coordinatesResult.Data;
            }
            else
            {
                result.Message = coordinatesResult.Message;
                return result;
            }
            result.Success = true;
            return result;
        }
    }
}
