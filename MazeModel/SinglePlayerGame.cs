using MazeLib;
using SearchAlgorithmsLib;


namespace MazeModel
{
    /// <summary>
    /// Holds info about single player game. 
    /// </summary>
    class SinglePlayerGame
    {
        /// <summary>
        /// Maze information to hold data about a maze.
        /// </summary>
        public Model.MazeInfo MazeInfo { get; set; }

        /// <summary>
        /// The maze of the game.
        /// </summary>
        public Maze Maze => MazeInfo.Maze;

        /// <summary>
        /// The maze's solution.
        /// </summary>
        public Solution<Position> Solution => MazeInfo.Solution;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="maze">Maze of the game.</param>
        public SinglePlayerGame(Maze maze)
        {
            this.MazeInfo = new Model.MazeInfo(maze);
        }
    }
}