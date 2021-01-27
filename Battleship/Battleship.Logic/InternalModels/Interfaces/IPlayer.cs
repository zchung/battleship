
using Battleship.Data.Entities;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;

namespace Battleship.Logic.InternalModels.Interfaces
{
    public interface IPlayer
    {
        int PlayerId { get; }
        List<ShipViewModel> GetShipViewModelsFromString(Game game);
        List<CoordinatesViewModel> GetAttemptedCoordinatesFromString(Game game);
        void SetShipViewModelJSON(GameViewModel gameViewModel, Game game);
        void SetAttemptedCoordinatesJSON(GameViewModel gameViewModel, Game game);
    }
}
