using MazeLib;
using System.Net.Sockets;

namespace MazeModel
{
    /// <summary>
    /// Holds info aboute multiplayer game.
    /// </summary>
    class MultiPlayerGame : SinglePlayerGame
    {
        /// <summary>
        /// Information about the player which created the game.
        /// </summary>
        public PlayerInfo Host { get; set; }

        /// <summary>
        /// Information about the player which joined to the game.
        /// </summary>
        public PlayerInfo Guest { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maze">The maze of the game.</param>
        /// <param name="player">The player which created the game.</param>
        /// <param name="location">The starting-location of the player.</param>
        public MultiPlayerGame(Maze maze, string playerId, Position location) : base(maze)
        {
            this.Host = new PlayerInfo(playerId, location);
        }

        /// <summary>
        /// Get the player-info of the given player.
        /// </summary>
        /// <param name="player">Player to find.</param>
        /// <returns> PlayerInfo of the specified player.</returns>
        public PlayerInfo GetPlayer(string playerId)
        {
            // Check if the player is the host or the guest.
            if (Host.PlayerId == playerId)
                return Host;
            if (Guest.PlayerId == playerId)
                return Guest;
            return null;
        }

        /// <summary>
        /// Given a player returns its competitor in the game.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <returns>Player's competitor.</returns>
        public PlayerInfo GetCompetitorOf(string playerId)
        {
            // Check if the given player is the host or the guest.
            if (Host.PlayerId == playerId)
                return Guest;
            if (Guest.PlayerId == playerId)
                return Host;
            return null;
        }
    }
}