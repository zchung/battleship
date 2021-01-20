using Battleship.Data.Models;
using Battleship.Logic.Constants;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Logic.Services
{
    public class CoordinatesValidationService : ICoordinatesValidationService
    {

        public Result ValidateCoordinatesInGameSpace(List<CoordinatesViewModel> coordinates)
        {
            string errorMessage = "Cannot place here, ship is out of game space";
            Result result = new Result();
            foreach (var item in coordinates)
            {
                if (item.XPosition > CoordinatesConstants.MAX_X_COORDINATES)
                {
                    result.Message = errorMessage;
                    return result;
                }
                if (item.YPosition > CoordinatesConstants.MAX_Y_COORDINATES)
                {
                    result.Message = errorMessage;
                    return result;
                }
            }
            result.Success = true;
            return result;
        }
        public Result ValidateCoordinatesForOverlapping(List<CoordinatesViewModel> newCoordinates, List<CoordinatesViewModel> existingCoordinates)
        {
            Result result = new Result();
            foreach (var item in newCoordinates)
            {
                if (existingCoordinates.FirstOrDefault(coordinate => coordinate.XPosition == item.XPosition && coordinate.YPosition == item.YPosition) != null)
                {
                    result.Message = "Cannot place here, it will overlap with another ship";
                    return result;
                }
            }
            result.Success = true;
            return result;
        }
    }
}
