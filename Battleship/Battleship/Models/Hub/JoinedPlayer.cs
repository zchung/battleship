
namespace Battleship.Models.Hub
{
    public class JoinedPlayer
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }

        public JoinedPlayer(int gameId, int playerId)
        {
            GameId = gameId;
            PlayerId = playerId;
        }
    }
}
