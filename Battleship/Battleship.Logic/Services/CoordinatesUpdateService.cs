using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Services
{
    public class CoordinatesUpdateService : ICoordinatesUpdateService
    {
        private readonly ICoordinatesFactory _coordinatesFactory;
        private readonly ICoordinatesValidationService _coordinatesValidationService;

        public CoordinatesUpdateService(ICoordinatesFactory coordinatesFactory, ICoordinatesValidationService coordinatesValidationService)
        {
            _coordinatesFactory = coordinatesFactory;
            _coordinatesValidationService = coordinatesValidationService;
        }
        public Result<List<CoordinatesViewModel>> UpdateNewCoordinates(CoordinatesViewModel startPosition, int size, ShipOrientationType shipOrientationType, List<CoordinatesViewModel> existingCoordintes)
        {
            Result<List<CoordinatesViewModel>> result = new Result<List<CoordinatesViewModel>>();
            var newCoordinates = _coordinatesFactory.GenerateNewCoordinates(startPosition, size, shipOrientationType);
            var outOfBoundsResult = _coordinatesValidationService.ValidateCoordinatesInGameSpace(newCoordinates);
            if (!outOfBoundsResult.Success)
            {
                result.Message = outOfBoundsResult.Message;
                return result;
            }
            var overlappingResult = _coordinatesValidationService.ValidateCoordinatesForOverlapping(newCoordinates, existingCoordintes);
            if (!overlappingResult.Success)
            {
                result.Message = overlappingResult.Message;
                return result;
            }
            result.Data = newCoordinates;
            result.Success = true;
            return result;
        }
    }
}
