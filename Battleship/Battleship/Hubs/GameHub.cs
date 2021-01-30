using Battleship.Logic.ViewModels;
using Battleship.Hubs.Interfaces;
using Battleship.Models.Hub;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Battleship.Data.Models;

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

        public async Task SendPlayerHasJoined(UpdatedPlayer updatedPlayer)
        {
            await Clients.All.SendPlayerHasJoined(updatedPlayer);
        }

        public async Task SendPlayerIsReady(UpdatedPlayer updatedPlayer)
        {
            await Clients.All.SendPlayerIsReady(updatedPlayer);
        }

        public async Task SendBothPlayersReady(UpdatedGame updatedGame)
        {
            await Clients.All.SendBothPlayersReady(updatedGame);
        }

        public async Task SendUpdateGameStatus(UpdatedGame updatedGame)
        {
            await Clients.All.SendUpdateGameStatus(updatedGame);
        }

        public async Task SendAttackPlayerCoordinates(AttackingPlayerResult attackingPlayerResult)
        {
            await Clients.All.SendAttackPlayerCoordinates(attackingPlayerResult);
        }
    }
}
