
using Battleship.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Battleship.Controllers.Interfaces
{
    public interface IShipController
    {
        Task<IActionResult> UpdateShipPosition(UpdateShipPositionRequest updateShipPositionRequest);
    }
}
