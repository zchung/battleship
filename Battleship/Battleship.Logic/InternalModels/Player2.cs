
using Battleship.Data.Entities;
using Battleship.Logic.InternalModels.Interfaces;
using Battleship.Logic.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Battleship.Logic.InternalModels
{
    public class Player2 : IPlayer
    {
        public int PlayerId => 2;

        public void SetShipViewModelJSON(GameViewModel gameViewModel, Game game)
        {
            game.Player2ShipsJSON = JsonConvert.SerializeObject(gameViewModel.Ships);
        }

        public void SetAttemptedCoordinatesJSON(GameViewModel gameViewModel, Game game)
        {
            game.Player2AttemptedCoordinatesJSON = JsonConvert.SerializeObject(gameViewModel.AttemptedCoordinates);
        }

        public List<ShipViewModel> GetShipViewModelsFromString(Game game)
        {
            return JsonConvert.DeserializeObject<List<ShipViewModel>>(game.Player2ShipsJSON);
        }

        public List<CoordinatesViewModel> GetAttemptedCoordinatesFromString(Game game)
        {
            return JsonConvert.DeserializeObject<List<CoordinatesViewModel>>(game.Player2AttemptedCoordinatesJSON);
        }
    }
}
