using Battleship.Logic.ViewModels;
using Battleship.Models.Hub;
using System.Threading.Tasks;

namespace Battleship.Hubs.Interfaces
{
    public interface IGameHub
    {
        Task SendNewGame(GameListViewModel game);
        Task RemoveGame(GameListViewModel game);
        Task SendPlayerHasJoined(JoinedPlayer joinedPlayer);
    }
}
