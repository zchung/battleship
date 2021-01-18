
using Battleship.Controllers.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Logic.Services.Interfaces;
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase, IGameController
    {
        private readonly IGameDbService _gameDbService;

        public GameController(IGameDbService gameDbService)
        {
            _gameDbService = gameDbService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(CreateGameRequest createGameRequest)
        {
            Game game = new Game { GameStatus = GameStatus.Active, Description = createGameRequest.Description };
            var result = await _gameDbService.Create(game);
            return Ok(result);
        }
    }
}
