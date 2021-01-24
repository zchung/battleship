using Battleship.Data.Enums;

namespace Battleship.Models.Requests
{
    public class SetGameStatusRequest
    {
        public int GameId { get; set; }
        public GameStatus GameStatus { get; set; }
    }
}
