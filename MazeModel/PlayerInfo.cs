using System.Net.Sockets;
using MazeLib;

namespace MazeModel
{
    /// <summary>
    /// Holds info about the player.
    /// </summary>
    class PlayerInfo
    {
        /// <summary>
        /// The player.
        /// </summary>
        public TcpClient Player { get; set; }

        /// <summary>
        /// Player's location.
        /// </summary>
        public Position Location { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <param name="location">It's location.</param>
        public PlayerInfo(TcpClient player, Position location)
        {
            this.Player = player;
            this.Location = location;
        }

        /// <summary>
        /// Advance the player in the given direction.
        /// </summary>
        /// <param name="maze">The maze of the game that the player participates.</param>
        /// <param name="direction">Direction to move.</param>
        /// <returns>True for valid move, false otherwise.</returns>
        public bool Move(Maze maze, string direction)
        {
            // Takes out player current position.
            int currentRow = this.Location.Row;
            int currentCol = this.Location.Col;

            // Right
            if (direction.Equals("right") && currentCol < maze.Cols - 1 &&
                maze[currentRow, currentCol + 1] == CellType.Free)
            {
                this.Location = new Position(currentRow, currentCol + 1);
            }
            // Left
            else if (direction.Equals("left") && currentCol > 0 &&
                     maze[currentRow, currentCol - 1] == CellType.Free)
            {
                this.Location = new Position(currentRow, currentCol - 1);
            }
            // Up
            else if (direction.Equals("up") && currentRow > 0 &&
                     maze[currentRow - 1, currentCol] == CellType.Free)
            {
                this.Location = new Position(currentRow - 1, currentCol);
            }
            // Down
            else if (direction.Equals("down") && currentRow < maze.Rows - 1 &&
                     maze[currentRow + 1, currentCol] == CellType.Free)
            {
                this.Location = new Position(currentRow + 1, currentCol);
            }
            else
            {
                // Invalid direction.
                return false;
            }
            return true;
        }
    }
}