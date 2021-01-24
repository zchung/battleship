
namespace Battleship.Models.Hub
{
    public class UpdatedPlayer
    {
        public int GameId { get; set; }
        public int PlayerId { get; set; }
        public bool BothPlayersReady { get; set; }

        public UpdatedPlayer(int gameId, int playerId, bool bothPlayersReady)
        {
            GameId = gameId;
            PlayerId = playerId;
            BothPlayersReady = bothPlayersReady;
        }
    }
}
