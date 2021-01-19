
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Data.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IGameDbService
    {
        Task<Result<Game>> Create(Game game);

        Task<Result<IEnumerable<GameListViewModel>>> GetActiveGames();
        Result<Game> GetById(int id);
        Task<Result> SaveChangesAsync();
    }
}
