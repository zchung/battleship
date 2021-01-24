using Battleship.Logic.ViewModels;
using Battleship.Models.Hub;
using System.Threading.Tasks;

namespace Battleship.Hubs.Interfaces
{
    public interface IGameHub
    {
        Task SendNewGame(List game);
        Task RemoveGame(List game);
        Task SendPlayerHasJoined(UpdatedPlayer updatedPlayer);
        Task SendPlayerIsReady(UpdatedPlayer updatedPlayer);
        Task SendBothPlayersReady(UpdatedGame updatedGame);
        Task SendGameHasStarted(UpdatedGame updatedGame);
    }
}
