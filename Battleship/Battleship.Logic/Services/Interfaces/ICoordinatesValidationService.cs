
using Battleship.Data.Models;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.Services.Interfaces
{
    public interface ICoordinatesValidationService
    {
        Result ValidateCoordinatesInGameSpace(List<CoordinatesViewModel> coordinates);
        Result ValidateCoordinatesForOverlapping(List<CoordinatesViewModel> newCoordinates, List<CoordinatesViewModel> existingCoordinates);
    }
}
