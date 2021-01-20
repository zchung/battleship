
namespace Battleship.Models.Hub
{
    public class UpdatedPlayer
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }

        public UpdatedPlayer(int gameId, int playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }
    }
}
