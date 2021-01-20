﻿
using System.Collections.Generic;

namespace Battleship.Logic.ViewModels
{
    public class GameViewModel
    {
        public int GameId { get; set; }
        public string Description { get; set; }
        public int PlayerId { get; set; }

        public List<ShipViewModel> Ships { get; set; }
    }
}