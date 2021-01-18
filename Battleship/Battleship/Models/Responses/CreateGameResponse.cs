using Battleship.Data.Models;

namespace Battleship.Models.Responses
{
    public class CreateGameResponse  : Result
    {
        public int GameId { get; set; }
    }
}
