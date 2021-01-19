
using Battleship.Data.Context.Interfaces;
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Battleship.Data.Enums;
using Microsoft.EntityFrameworkCore;

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
        public async Task<Result<Game>> Create(Game game)
        {
            Result<Game> result = new Result<Game>();
            try
            {
                await _battleshipDbContext.AddAsync(game);
                await _battleshipDbContext.SaveChangesAsync();
                result.Data = game;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "Error creating new Game";
                Console.WriteLine(ex.Message); //normally would log to a service here.
            }
            
            return result;
        }

        public async Task<Result<IEnumerable<Game>>> GetActiveGames()
        {
            Result<IEnumerable<Game>> result = new Result<IEnumerable<Game>>();
            try
            {
                result.Data = await _battleshipDbContext.Games.Where(x => x.GameStatus == GameStatus.Active).ToListAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Message = "Error getting games";
                Console.WriteLine(ex.Message); //normally would log to a service here.
            }
            return result;
        }
    }
}
