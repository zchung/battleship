
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.InternalModels;
using Battleship.Logic.InternalModels.Interfaces;

namespace Battleship.Logic.Factories
{
    public class PlayerFactory : IPlayerFactory
    {
        public IPlayer GetPlayer(int playerId)
        {
            if (playerId == 1)
            {
                return new Player1();
            } 
            else
            {
                return new Player2();
            }
        }
    }
}
