
using Battleship.Data.Enums;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Logic.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Description { get; set; }
        public int PlayerId { get; set; }
        public int? CurrentPlayerIdTurn { get; set; }
        public List<ShipViewModel> Ships { get; set; }
        public List<CoordinatesViewModel> AttemptedCoordinates { get; set; }
        public PlayerStatus Player1Status { get; set; }
        public PlayerStatus Player2Status { get; set; }
        public bool GameReadyToStart { 
            get
            {
                return Player1Status == PlayerStatus.Ready && Player2Status == PlayerStatus.Ready;
            }
        }
        public bool AllShipsPlaced()
        {
            return Ships.TrueForAll(s => s.IsPlaced());
        }
        public bool AllShipsDestroyed()
        {
            return Ships.TrueForAll(x => x.IsDestroyed());
        }
        public bool AttackPlayer(CoordinatesViewModel coordinatesViewModel)
        {
            bool hit = false;
            foreach (var ship in Ships)
            {
                CoordinatesViewModel matchingCoordinates = ship.FindCoordinatesInShip(coordinatesViewModel);
                if (matchingCoordinates != null)
                {
                    matchingCoordinates.Hit = coordinatesViewModel.Hit = true;
                    hit = true;
                    break;
                }
                else
                {
                    coordinatesViewModel.Hit = false;
                }
            }
            return hit;
        }

        public bool CheckIfCoordinatesHasAlreadyBeenTried(CoordinatesViewModel coordinatesViewModel)
        {
            return AttemptedCoordinates?.FirstOrDefault(coordinates => coordinates.XPosition == coordinatesViewModel.XPosition && 
                                                        coordinates.YPosition == coordinatesViewModel.YPosition) != null;
        }
    }
}
