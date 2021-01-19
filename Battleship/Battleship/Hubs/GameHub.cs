using Battleship.Data.Entities;
using Battleship.Hubs.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Battleship.Hubs
{
    public class GameHub : Hub<IGameHub>
    {
        public async Task SendNewGame(Game game)
        {
            await Clients.All.SendNewGame(game);
        }
    }
}
