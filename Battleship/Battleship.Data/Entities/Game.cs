﻿
using Battleship.Data.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Battleship.Data.Entities
{
    public class Game
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GameId { get; set; }
        public string Description { get; set; }
        public GameStatus GameStatus { get; set; }
        public int? CurrentPlayerIdTurn { get; set; }
        public string Player1ShipsJSON { get; set; }
        public string Player2ShipsJSON { get; set; }
        public string Player1AttemptedCoordinatesJSON { get; set; }
        public string Player2AttemptedCoordinatesJSON { get; set; }
        public PlayerStatus Player1Status { get; set; }
        public PlayerStatus Player2Status { get; set; }

        [NotMapped]
        public bool BothPlayersReady 
        {
            get { return Player1Status.Equals(PlayerStatus.Ready) && Player2Status.Equals(PlayerStatus.Ready); }
        }
    }
}
