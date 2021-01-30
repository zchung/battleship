using Battleship.Data.Models;
using Battleship.Logic.ViewModels;
using Battleship.Models.Hub;
using System.Threading.Tasks;

namespace Battleship.Hubs.Interfaces
{
    public interface IGameHub
    {
        Task SendNewGame(GameListViewModel game);
        Task RemoveGame(GameListViewModel game);
        Task SendPlayerHasJoined(UpdatedPlayer updatedPlayer);
        Task SendPlayerIsReady(UpdatedPlayer updatedPlayer);
        Task SendBothPlayersReady(UpdatedGame updatedGame);
        Task SendUpdateGameStatus(UpdatedGame updatedGame);
        Task SendAttackPlayerCoordinates(AttackingPlayerResult attackingPlayerResult);
    }
}
