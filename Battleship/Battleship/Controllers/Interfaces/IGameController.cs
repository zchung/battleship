
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Battleship.Controllers.Interfaces
{
    public interface IGameController
    {
        Task<IActionResult> Create(CreateGameRequest createGameRequest);

        Task<IActionResult> GetActive();
        Task<IActionResult> Join(JoinGameRequest joinGameRequest);
        IActionResult Get(int gameId, int playerId);
        Task<IActionResult> SetPlayerToPrepared(GamePlayerRequest gamePlayerRequest);
    }
}
