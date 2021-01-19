
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Hubs;
using Battleship.Hubs.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase, IGameController
    {
        private readonly IGameDbService _gameDbService;
        private readonly IHubContext<GameHub, IGameHub> _hubContext;

        public GameController(IGameDbService gameDbService, IHubContext<GameHub, IGameHub> hubContext)
        {
            _gameDbService = gameDbService;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CreateGameRequest createGameRequest)
        {
            Game game = new Game { GameStatus = GameStatus.Active, Description = createGameRequest.Description };
            var result = await _gameDbService.Create(game);
            await _hubContext.Clients.All.SendNewGame(result.Data);
            return Ok(result);
        }

        [HttpGet]
        [Route("getactive")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _gameDbService.GetActiveGames();

            return Ok(result);
        }
    }
}
