
using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Battleship.Data.Enums;

namespace Battleship.Logic.Services
{
    public class GameDbStaticService : IGameDbService
    {
        public object lockingObject = new object();
        public List<Game> internalList = new List<Game>();
        public Task<Result<Game>> Create(Game game)
        {
            lock (lockingObject)
            {
                int gameId = 1;
                if (internalList.Any())
                {
                    gameId = internalList.Max(x => x.GameId) + 1;
                }
                game.GameId = gameId;
                internalList.Add(game);
                return Task.FromResult(new Result<Game> { Success = true, Data = game });
            }          
        }

        public Task<Result<IEnumerable<GameListViewModel>>> GetActiveGames()
        {
            lock (lockingObject)
            {
                return Task.FromResult(new Result<IEnumerable<GameListViewModel>>
                {
                    Data = internalList.Where(x => x.GameStatus == GameStatus.Active)
                .Select(s => new GameListViewModel
                {
                    Description = s.Description,
                    GameId = s.GameId
                }),
                    Success = true
                });
            } 
        }

        public Result<Game> GetById(int id)
        {
            lock (lockingObject)
            {
                var result = new Result<Game>();
                result.Data = internalList.FirstOrDefault(x => x.GameId == id);
                if (result.Data == null)
                {
                    result.Message = "No Game found";
                }
                else
                {
                    result.Success = true;
                }
                return result;
            }           
        }

        public Result Update(Game game)

        {
            lock (lockingObject)
            {
                Result result = new Result();
                result.Success = true;
                var matchingGame = internalList.FirstOrDefault(x => x.GameId == game.GameId);
                if (matchingGame != null)
                {
                    matchingGame.CurrentPlayerIdTurn = game.CurrentPlayerIdTurn;
                    matchingGame.Description = game.Description;
                    matchingGame.GameStatus = game.GameStatus;
                    matchingGame.Player1AttemptedCoordinatesJSON = game.Player1AttemptedCoordinatesJSON;
                    matchingGame.Player1ShipsJSON = game.Player1ShipsJSON;
                    matchingGame.Player2AttemptedCoordinatesJSON = game.Player2AttemptedCoordinatesJSON;
                    matchingGame.Player2ShipsJSON = game.Player2ShipsJSON;
                    matchingGame.Player1Status = game.Player1Status;
                    matchingGame.Player2Status = game.Player2Status;
                }

                return result;
            }           
        }

        public Task<Result> SaveChangesAsync()
        {
            return Task.FromResult(new Result { Success = true });
        }
    }
}
