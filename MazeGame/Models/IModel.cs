using System.Net.Sockets;
using MazeLib;
using SearchAlgorithmsLib;

namespace ServerProject.ModelLib
{
    /// <summary>
    /// Model of server.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Generate a new maze with the given arguments.
        /// </summary>
        /// <param name="nameOfGame">Name of the game.</param>
        /// <param name="rows">Number of rows taht will be in the maze.</param>
        /// <param name="cols">Number of cols taht will be in the maze.</param>
        /// <returns>Maze that was generated.</returns>
        Maze GenerateMaze(string nameOfGame, int rows, int cols);

        /// <summary>
        /// Solve a maze.
        /// </summary>
        /// <param name="nameOfGame">Name of maze to solve.</param>
        /// <param name="algorithm">Number that represents the algorithm to solve the maze with.</param>
        /// <returns></returns>
        Solution<Position> SolveMaze(string nameOfGame, int algorithm);

        /// <summary>
        /// Start a new multiplayer-game.
        /// </summary>
        /// <param name="nameOfGame">Name of the mew game.</param>
        /// <param name="rows">Number of rows taht will be in the maze.</param>
        /// <param name="cols">Number of cols taht will be in the maze.</param>
        /// <param name="player">Host of the game</param>
        /// <returns>Maze that was generated to the game.</returns>
        Maze StartGame(string nameOfGame, int rows, int cols, TcpClient player);

        /// <summary>
        /// Get all the available games that some player can join to.
        /// </summary>
        /// <returns>Array of string, which each is name of some game.</returns>
        string[] GetAvailableGames();

        /// <summary>
        /// Add the given player to the specified game.
        /// </summary>
        /// <param name="nameOfGame">The name of game the player want to join to.</param>
        /// <param name="player">The player.</param>
        /// <returns>The maze of the game that the players participate.</returns>
        Maze JoinTo(string nameOfGame, TcpClient player);

        /// <summary>
        /// Play one move in some game.
        /// </summary>
        /// <param name="direction">String represents the direction that the player want to move to.</param>
        /// <param name="player">Player that sent the message.</param>
        /// <returns>String represents the information of the movement.</returns>
        string Play(string direction, TcpClient player);

        /// <summary>
        /// Close a multiplayer game.
        /// </summary>
        /// <param name="nameOfGame">The name of game that will be closed.</param>
        void Close(string nameOfGame);

        /// <summary>
        /// Check if some game begun.
        /// </summary>
        /// <param name="nameOfGame">The name of game that needs to check.</param>
        /// <returns>True if the game begun, false otherwise.</returns>
        bool IsGameBegun(string nameOfGame);

        /// <summary>
        /// Check if the some client is in the middle of multiplayer game.
        /// </summary>
        /// <param name="client">The client to check for.</param>
        /// <returns>True if the client is inside a multiplayer game, false otherwise.</returns>
        bool IsClientInGame(TcpClient client);

        /// <summary>
        /// Get the competitor of some player in multiplayer game.
        /// </summary>
        /// <param name="player">Player.</param>
        /// <returns>Its competitor, null if the given client is not in any game.</returns>
        TcpClient GetCompetitorOf(TcpClient player);
    }
}