using MazeLib;
using System.Net.Sockets;

namespace ServerProject.ModelLib
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
        public MultiPlayerGame(Maze maze, TcpClient player, Position location) : base(maze)
        {
            this.Host = new PlayerInfo(player, location);
        }

        /// <summary>
        /// Get the player-info of the given player.
        /// </summary>
        /// <param name="player">Player to find.</param>
        /// <returns> PlayerInfo of the specified player.</returns>
        public PlayerInfo GetPlayer(TcpClient player)
        {
            // Check if the player is the host or the guest.
            if (Host.Player == player)
                return Host;
            if (Guest.Player == player)
                return Guest;
            return null;
        }

        /// <summary>
        /// Given a player returns its competitor in the game.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <returns>Player's competitor.</returns>
        public PlayerInfo GetCompetitorOf(TcpClient player)
        {
            // Check if the given player is the host or the guest.
            if (Host.Player == player)
                return Guest;
            if (Guest.Player == player)
                return Host;
            return null;
        }
    }
}