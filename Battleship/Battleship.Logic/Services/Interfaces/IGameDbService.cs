
using Battleship.Data.Entities;
using Battleship.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IGameDbService
    {
        Task<Result<Game>> Create(Game game);

        Task<Result<IEnumerable<Game>>> GetActiveGames();
    }
}
