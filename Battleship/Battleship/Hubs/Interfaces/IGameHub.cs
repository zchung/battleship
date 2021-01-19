
using Battleship.Data.Entities;
using System.Threading.Tasks;

namespace Battleship.Hubs.Interfaces
{
    public interface IGameHub
    {
        Task SendNewGame(Game game);
    }
}
