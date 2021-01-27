
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.InternalModels.Interfaces;

namespace Battleship.Logic.InternalModels
{
    public class PlanningGame : IGameType
    {
        private readonly Game _game;
        public PlanningGame(Game game)
        {
            _game = game;
        }
        public void HandleStatusUpdate()
        {
            _game.GameStatus = GameStatus.Planning;
            _game.Player2Status = PlayerStatus.Preparing;
        }
    }
}
