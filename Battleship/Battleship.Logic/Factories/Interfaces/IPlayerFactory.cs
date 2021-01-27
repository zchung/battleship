
using Battleship.Logic.InternalModels.Interfaces;

namespace Battleship.Logic.Factories.Interfaces
{
    public interface IPlayerFactory
    {
        IPlayer GetPlayer(int playerId);
    }
}
