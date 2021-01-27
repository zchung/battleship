
namespace Battleship.Models.Hub
{
    public class UpdatedGame
    {
        public int GameId { get; set; }
        public int? CurrentPlayerIdTurn { get; set; }

        public UpdatedGame(int gameId, int? currentPlayerIdTurn)
        {
            GameId = gameId;
            CurrentPlayerIdTurn = currentPlayerIdTurn;
        }
    }
}
