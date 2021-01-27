
using Battleship.Data.Entities;
using Battleship.Logic.InternalModels.Interfaces;
using Battleship.Logic.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Battleship.Logic.InternalModels
{
    public class Player1 : IPlayer
    {
        public int PlayerId => 1;

        public void SetShipViewModelJSON(GameViewModel gameViewModel, Game game)
        {
            game.Player1ShipsJSON = JsonConvert.SerializeObject(gameViewModel.Ships);
        }
        public void SetAttemptedCoordinatesJSON(GameViewModel gameViewModel, Game game)
        {
            game.Player1AttemptedCoordinatesJSON = JsonConvert.SerializeObject(gameViewModel.AttemptedCoordinates);
        }

        public List<ShipViewModel> GetShipViewModelsFromString(Game game)
        {
            return JsonConvert.DeserializeObject<List<ShipViewModel>>(game.Player1ShipsJSON);
        }

        public List<CoordinatesViewModel> GetAttemptedCoordinatesFromString(Game game)
        {
            return JsonConvert.DeserializeObject<List<CoordinatesViewModel>>(game.Player1AttemptedCoordinatesJSON);
        }

    }
}
