using Battleship.Data.Entities;
using Battleship.Data.Models;
using Battleship.Logic.Enums;
using Battleship.Logic.Factories;
using Battleship.Logic.Factories.Interfaces;
using Battleship.Logic.Services;
using Battleship.Logic.Services.Interfaces;
using Battleship.Logic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship.Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            int roundsToPlay = 10;
            Random random = new Random();
            List<string> totalResults = new List<string>();
            for (int i = 0; i < roundsToPlay; i++)
            {
                string gameName = $"Game: {i + 1}";
                System.Console.WriteLine(gameName);
                string gameResult = PlayGame(random);
                totalResults.Add($"{gameName}: {gameResult}");
            }
            foreach (var result in totalResults)
            {
                System.Console.WriteLine(result);
            }
        }

        private static string PlayGame(Random rnd)
        {
            IShipFactory _shipFactory = new ShipFactory();
            IRandomGeneratorFactory _randomGeneratorFactory = new RandomGeneratorFactory();
            IPlayerFactory _playerFactory = new PlayerFactory();
            IGameFactory _gameFactory = new GameFactory(_shipFactory, _randomGeneratorFactory, _playerFactory);
            ICoordinatesFactory _coordinatesFactory = new CoordinatesFactory();
            ICoordinatesValidationService _coordinatesValidationService = new CoordinatesValidationService();
            ICoordinatesUpdateService _coordinatesUpdateService = new CoordinatesUpdateService(_coordinatesFactory, _coordinatesValidationService);

            System.Console.WriteLine("Setting up the game");
            Game game = _gameFactory.CreateNewGame("Test Game");

            System.Console.WriteLine("Setting up the players");
            GameViewModel player1Game = _gameFactory.GetGameViewModel(game, 1);
            GameViewModel player2Game = _gameFactory.GetGameViewModel(game, 2);

            System.Console.WriteLine("Setting up the ships player 1");
            GenerateRandomCoordinatesForShips(player1Game.Ships, _coordinatesFactory, _coordinatesUpdateService);
            System.Console.WriteLine("Setting up the ships player 2");
            GenerateRandomCoordinatesForShips(player2Game.Ships, _coordinatesFactory, _coordinatesUpdateService);

            // setting up list so the players dont pick the same coordinates twice.\
            PlayerCoordinates playerCoordinates1 = new PlayerCoordinates { GameViewModel = player1Game, CoordinatesViewModels = _coordinatesFactory.GenerateGameboard() };
            PlayerCoordinates playerCoordinates2 = new PlayerCoordinates { GameViewModel = player2Game, CoordinatesViewModels = _coordinatesFactory.GenerateGameboard() };
            int firstPlayerToStartId = rnd.Next(1, 3);
            System.Console.WriteLine($"Player {firstPlayerToStartId} starts");
            PlayerCoordinates firstPlayerCoordinates = null;
            PlayerCoordinates secondPlayerCoordinates = null;
            if (firstPlayerToStartId == 1)
            {
                firstPlayerCoordinates = playerCoordinates1;
                secondPlayerCoordinates = playerCoordinates2;
            }
            else
            {
                firstPlayerCoordinates = playerCoordinates2;
                secondPlayerCoordinates = playerCoordinates1;
            }
            int turnCounter;
            PlayRound(firstPlayerCoordinates, secondPlayerCoordinates, out turnCounter);

            string winner = firstPlayerCoordinates.AllShipsDestroyed ? $"Player {secondPlayerCoordinates.GameViewModel.PlayerId}" : $"Player {firstPlayerCoordinates.GameViewModel.PlayerId}";
            int playerShipsRemaining = firstPlayerCoordinates.AllShipsDestroyed ? secondPlayerCoordinates.GameViewModel.Ships.Count(x => !x.IsDestroyed()) : firstPlayerCoordinates.GameViewModel.Ships.Count(x => !x.IsDestroyed());
            string finalResult = $"The winner is {winner} they had {playerShipsRemaining} ships left, this game took {turnCounter} turns";
            System.Console.WriteLine(finalResult);
            return finalResult;
        }
        private static void PlayRound(PlayerCoordinates firstPlayer, PlayerCoordinates secondPlayer, out int turnCounter)
        {
            turnCounter = 0;
            Random rnd = new Random();
            do
            {
                string firstPlayerName = $"Player {firstPlayer.GameViewModel.PlayerId}";
                string secondPlayerName = $"Player {secondPlayer.GameViewModel.PlayerId}";
                turnCounter++;
                System.Console.WriteLine($"Turn: {turnCounter}");
                System.Console.WriteLine($"{firstPlayerName} has {firstPlayer.CoordinatesViewModels.Count} options left");
                System.Console.WriteLine($"{secondPlayerName} has {secondPlayer.CoordinatesViewModels.Count} options left");
                secondPlayer.AllShipsDestroyed = TakeTurn(secondPlayer.GameViewModel, firstPlayer.CoordinatesViewModels, firstPlayerName, rnd);
                if (!secondPlayer.AllShipsDestroyed)
                {
                    firstPlayer.AllShipsDestroyed = TakeTurn(firstPlayer.GameViewModel, secondPlayer.CoordinatesViewModels, secondPlayerName, rnd);
                }
            } while (!firstPlayer.AllShipsDestroyed && !secondPlayer.AllShipsDestroyed);
        }

        private static void GenerateRandomCoordinatesForShips(List<ShipViewModel> ships, ICoordinatesFactory _coordinatesFactory, ICoordinatesUpdateService _coordinatesUpdateService)
        {
            foreach (var ship in ships)
            {
                Result<List<CoordinatesViewModel>> result = null;
                do
                {
                    CoordinatesViewModel randomCoordinates = _coordinatesFactory.GetRandomCoordinate();
                    ShipOrientationType shipOrientationType = _coordinatesFactory.GetRandomOrientation();
                    List<CoordinatesViewModel> coordinatesOfOtherShips = ships.Where(x => x.ShipType != ship.ShipType).SelectMany(sm => sm.Coordinates).ToList();
                    result = _coordinatesUpdateService.UpdateNewCoordinates(randomCoordinates, ship.Size, shipOrientationType, coordinatesOfOtherShips);

                } while (!result.Success);
                ship.Coordinates = result.Data;
            }
        }

        private static bool TakeTurn(GameViewModel defender, List<CoordinatesViewModel> attackerSelectableCoordinates, string attackerName, Random rnd)
        {
            int random = rnd.Next(0, attackerSelectableCoordinates.Count - 1);
            CoordinatesViewModel rndCoordinatesViewModel = attackerSelectableCoordinates[random];
            attackerSelectableCoordinates.Remove(rndCoordinatesViewModel);

            bool hit = defender.AttackPlayer(rndCoordinatesViewModel);
            string resultText = hit ? "Hit" : "Miss";
            System.Console.WriteLine($"{attackerName} chooses x: {rndCoordinatesViewModel.XPosition} y: {rndCoordinatesViewModel.YPosition}, its a {resultText}");
            return defender.AllShipsDestroyed();
        }

    }
}
