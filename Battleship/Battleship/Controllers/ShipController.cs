using Battleship.Controllers.Interfaces;
using Battleship.Data.Models;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Battleship.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ShipController : ControllerBase, IShipController
    {
        private readonly IGameDbService _gameDbService;
        private readonly IGameFactory _gameFactory;
        private readonly IShipUpdateService _shipUpdateService;

        public ShipController(IGameDbService gameDbService, IGameFactory gameFactory, IShipUpdateService shipUpdateService)
        {
            _gameDbService = gameDbService;
            _gameFactory = gameFactory;
            _shipUpdateService = shipUpdateService;
        }
        [HttpPost]
        [Route("updateshipposition")]
        public async Task<IActionResult> UpdateShipPosition(UpdateShipPositionRequest updateShipPositionRequest)
        {
            Result<ShipViewModel> result = new Result<ShipViewModel>();
            var gameResult = _gameDbService.GetById(updateShipPositionRequest.GameId);
            if (gameResult.Success)
            {
                var gameViewModel = _gameFactory.GetGameViewModel(gameResult.Data, updateShipPositionRequest.PlayerId);
                var updateShipPositionResult =  _shipUpdateService.UpdateShipPosition(gameViewModel.Ships, updateShipPositionRequest.StartPosition, updateShipPositionRequest.ShipType, updateShipPositionRequest.ShipOrientationType);
                if (updateShipPositionResult.Success)
                {
                    if (updateShipPositionRequest.PlayerId == 1)
                    {
                        gameResult.Data.Player1ShipsJSON = JsonConvert.SerializeObject(gameViewModel.Ships);
                    }
                    else
                    {
                        gameResult.Data.Player2ShipsJSON = JsonConvert.SerializeObject(gameViewModel.Ships);
                    }
                    var saveResult = await _gameDbService.SaveChangesAsync();
                    if (saveResult.Success)
                    {

                        result.Success = true;
                        result.Data = gameViewModel.Ships.FirstOrDefault(x => x.ShipType == updateShipPositionRequest.ShipType);
                    }
                    else
                    {
                        result.Message = saveResult.Message;
                    }
                }
                else
                {
                    result.Message = updateShipPositionResult.Message;
                }
            }
            else
            {
                result.Message = gameResult.Message;
            }

            return Ok(result);
        }
    }
}
