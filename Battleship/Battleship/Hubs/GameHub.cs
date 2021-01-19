using Battleship.Data.Models.ViewModels;
using Battleship.Hubs.Interfaces;
using Battleship.Models.Hub;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Battleship.Hubs
{
    public class GameHub : Hub<IGameHub>
    {
        public async Task SendNewGame(GameListViewModel game)
        {
            await Clients.All.SendNewGame(game);
        }

        public async Task RemoveGame(GameListViewModel game)
        {
            await Clients.All.RemoveGame(game);
        }

        public async Task SendPlayerHasJoined(JoinedPlayer joinedPlayer)
        {
            await Clients.All.SendPlayerHasJoined(joinedPlayer);
        }
    }
}
