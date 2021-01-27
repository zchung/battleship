
using Battleship.Data.Entities;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels.Interfaces;
using System.Collections.Generic;

namespace Battleship.Logic.InternalModels
{
    public class StartedGame : IGameType
    {
        private readonly Game _game;
        private readonly IRandomGeneratorFactory _randomGeneratorFactory;
        public StartedGame(Game game, IRandomGeneratorFactory randomGeneratorFactory)
        {
            _game = game;
            _randomGeneratorFactory = randomGeneratorFactory;
        }
        public void HandleStatusUpdate()
        {
            List<int> playerIds = new List<int>
            { 
                1, 2
            };

            _game.CurrentPlayerIdTurn =  playerIds[_randomGeneratorFactory.GetRandomNumber(0, playerIds.Count)];

        }
    }
}
