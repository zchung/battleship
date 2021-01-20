﻿
using Battleship.Data.Entities;
using Battleship.Logic.ViewModels;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Data.Enums;
using Newtonsoft.Json;
using System.Collections.Generic;
using Battleship.Logic.Enums;

namespace Battleship.Logic.Factories
{
    public class GameFactory : IGameFactory
    {
        private readonly IShipFactory _shipFactory;
        public GameFactory(IShipFactory shipFactory)
        {
            _shipFactory = shipFactory;
        }
        public List GetGameListViewModel(Game game)
        {
            return new List
            {
                GameId = game.GameId,
                Description = game.Description
            };
        }

        public GameViewModel GetGameViewModel(Game game, int playerId)
        {
            var gameViewModel =  new GameViewModel
            {
                GameId = game.GameId,
                Description = game.Description,
                PlayerId = playerId,
                Ships = playerId == 1 ? JsonConvert.DeserializeObject<List<ShipViewModel>>(game.Player1ShipsJSON) : JsonConvert.DeserializeObject<List<ShipViewModel>>(game.Player2ShipsJSON),
                Player1Status = game.Player1Status,
                Player2Status = game.Player2Status
            };

            return gameViewModel;
        }

        public Game CreateNewGame(string description)
        {
            return new Game
            {
                Description = description,
                GameStatus = GameStatus.Active,
                Player1ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player2ShipsJSON = JsonConvert.SerializeObject(_shipFactory.GetDefaultShips()),
                Player1Status = PlayerStatus.Preparing,
                Player2Status = PlayerStatus.Empty
            };
        }
    }
}
