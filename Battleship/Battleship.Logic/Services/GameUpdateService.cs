﻿using Battleship.Data.Entities;
using Battleship.Data.Enums;
using Battleship.Data.Models;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System.Threading.Tasks;

namespace Battleship.Logic.Services
{
    public class GameUpdateService : IGameUpdateService
    {
        private readonly IGameDbService _gameDbService;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerFactory _playerFactory;
        public GameUpdateService(IGameDbService gameDbService, IGameFactory gameFactory, IPlayerFactory playerFactory)
        {
            _gameDbService = gameDbService;
            _gameFactory = gameFactory;
            _playerFactory = playerFactory;
        }

        public async Task<Result<Game>> UpdateGameAfterPlayerJoins(int gameId)
        {
            Result<Game> gameResult = _gameDbService.GetById(gameId);

            if (gameResult.Data != null && gameResult.Success)
            {
                var gameType = _gameFactory.GetGameType(gameResult.Data, GameStatus.Planning);
                gameType.HandleStatusUpdate();
                _gameDbService.Update(gameResult.Data);
                var saveResult = await _gameDbService.SaveChangesAsync();
                if (saveResult.Success)
                {

                    gameResult.Success = true;
                }
                else
                {
                    gameResult.Success = false;
                    gameResult.Message = saveResult.Message;
                }
            }
            else
            {
                gameResult.Message = "Unable to update game after joining game.";
            }

            return gameResult;
        }

        public async Task<Result<Game>> UpdateGameStatus(int gameId, GameStatus status)
        {
            Result<Game> gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Success)
            {
                gameResult.Data.GameStatus = status;
                var gameType = _gameFactory.GetGameType(gameResult.Data, status);
                gameType.HandleStatusUpdate();
                _gameDbService.Update(gameResult.Data);
                var saveResult = await _gameDbService.SaveChangesAsync();
                if (saveResult.Success)
                {
                    gameResult.Success = true;
                }
                else
                {
                    gameResult.Success = false;
                    gameResult.Message = saveResult.Message;
                }
            }

            return gameResult;
        }

        public async Task<Result<Game>> UpdatePlayerToReady(int gameId, int playerId)
        {
            Result<Game> gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Data != null && gameResult.Success)
            {
                var gameViewModel = _gameFactory.GetGameViewModel(gameResult.Data, playerId);
                if (gameViewModel.AllShipsPlaced())
                {
                     
                    if (playerId == 1)
                    {
                        gameResult.Data.Player1Status = PlayerStatus.Ready;
                    }
                    else
                    {
                        gameResult.Data.Player2Status = PlayerStatus.Ready;
                    }
                    _gameDbService.Update(gameResult.Data);
                    var saveResult = await _gameDbService.SaveChangesAsync();
                    if (saveResult.Success)
                    {
                        gameResult.Success = true;
                    }
                    else
                    {
                        gameResult.Message = "Unable to update Status";
                    }
                }
                else
                {
                    gameResult.Success = false;
                    gameResult.Message = "You must place all ships before you are prepared.";
                }
            }

            return gameResult;
        }
        public async Task<AttackingPlayerResult> ResolvePlayerAttack(int gameId, int playerIdAttacking, int playerToAttackId, CoordinatesViewModel coordinatesViewModel)
        {
            var attackingPlayerResult = new AttackingPlayerResult(gameId, playerIdAttacking, playerToAttackId, coordinatesViewModel, false, "");
            var attackingPlayer = _playerFactory.GetPlayer(playerIdAttacking);
            var defendingPlayer = _playerFactory.GetPlayer(playerToAttackId);
            var gameResult = _gameDbService.GetById(gameId);
            if (gameResult.Success)
            {
                var attackingPlayerGameViewModel = _gameFactory.GetGameViewModel(gameResult.Data, playerIdAttacking);
                if (!attackingPlayerGameViewModel.CheckIfCoordinatesHasAlreadyBeenTried(coordinatesViewModel))
                {
                    attackingPlayerGameViewModel.AttemptedCoordinates.Add(coordinatesViewModel);
                }
                else
                {

                    attackingPlayerResult.Message = "This coordinate has already been selected";
                    return attackingPlayerResult;
                }
                string finalMessage = $"Player {playerIdAttacking} {{0}} Player {playerToAttackId}";
                var defendingPlayerGameViewModel = _gameFactory.GetGameViewModel(gameResult.Data, playerToAttackId);
                if (defendingPlayerGameViewModel.AttackPlayer(coordinatesViewModel))
                {
                    attackingPlayerResult.Message = string.Format(finalMessage, "Hits");
                }
                else
                {
                    attackingPlayerResult.Message = string.Format(finalMessage, "Misses");
                }

                gameResult.Data.CurrentPlayerIdTurn = defendingPlayer.PlayerId;

                attackingPlayer.SetAttemptedCoordinatesJSON(attackingPlayerGameViewModel, gameResult.Data);
                defendingPlayer.SetShipViewModelJSON(defendingPlayerGameViewModel, gameResult.Data);
                if (defendingPlayerGameViewModel.AllShipsDestroyed())
                {
                    attackingPlayerResult.WinnerOfGamePlayerId = attackingPlayer.PlayerId;
                    gameResult.Data.GameStatus = GameStatus.Completed;
                }
                _gameDbService.Update(gameResult.Data);
                var saveResult = await _gameDbService.SaveChangesAsync();
                
                if (saveResult.Success)
                {
                    attackingPlayerResult.Success = true;
                }
            }

            return attackingPlayerResult;

        }
    }
}
