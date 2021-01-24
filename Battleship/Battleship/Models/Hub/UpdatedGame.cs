
namespace Battleship.Models.Hub
{
    public class UpdatedGame
    {
        public int GameId { get; set; }

        public UpdatedGame(int gameId)
        {
            GameId = gameId;
        }
    }
}
