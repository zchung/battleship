﻿
using Battleship.Data.Entities;
using Battleship.Data.Models;
using System.Threading.Tasks;

namespace Battleship.Logic.Services.Interfaces
{
    public interface IGameUpdateService
    {
        Task<Result<Game>> UpdateGameAfterPlayerJoins(int gameId);
        Task<Result> UpdatePlayerToPrepared(int gameId, int playerId);
    }
}
