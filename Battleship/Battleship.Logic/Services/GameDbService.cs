
using Battleship.Data.Context.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Battleship.Logic.Services
{
    /// <summary>
    /// This class contains all logic related to database interaction
    /// </summary>
    public class GameDbService : IGameDbService
    {
        private readonly IBattleshipDbContext _battleshipDbContext;

        public GameDbService(IBattleshipDbContext battleshipDbContext)
        {
            _battleshipDbContext = battleshipDbContext;
        }
        public async Task<Result<int>> Create(Game game)
        {
            Result<int> result = new Result<int>();
            try
            {
                await _battleshipDbContext.AddAsync(game);
                await _battleshipDbContext.SaveChangesAsync();
                result.Data = game.GameId;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "Error creating new Game";
                Console.WriteLine(ex.Message); //normally would log to a service here.
            }
            
            return result;
        }
    }
}
