using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using MazeGeneratorLib;
using MazeLib;
using MazeObjectAdapterLib;
using SearchAlgorithmsLib;


namespace MazeModel
{
    /// <summary>
    /// This class responsible of all tha logic of the game.
    /// </summary>
    public class Model : IModel
    {
        /// <summary>
        /// Dictionary that maps name of game, that players can join, to its multiplayer-game.
        /// </summary>
        private Dictionary<string, MultiPlayerGame> availablesMPGames;

        /// <summary>
        /// Dictionary that maps name of game, that players can not join (has 2 players inside),
        /// to its multiplayer-game.
        /// </summary>
        private Dictionary<string, MultiPlayerGame> unAvailablesMPGames;

        /// <summary>
        /// Dictionary that maps name of game to its singleplayer-game.
        /// </summary>
        private Dictionary<string, SinglePlayerGame> SPGames;

        /// <summary>
        /// Dictionary that maps client to the multiplayer game that he participates.
        /// </summary>
        private Dictionary<TcpClient, MultiPlayerGame> playerToGame;

        /// <summary>
        /// Constructor.
        /// </summary>
        public Model()
        {
            availablesMPGames = new Dictionary<string, MultiPlayerGame>();
            unAvailablesMPGames = new Dictionary<string, MultiPlayerGame>();
            SPGames = new Dictionary<string, SinglePlayerGame>();
            playerToGame = new Dictionary<TcpClient, MultiPlayerGame>();
        }

        public Maze GenerateMaze(string nameOfGame, int rows, int cols)
        {
            if (SPGames.ContainsKey(nameOfGame))
            {
                // The game is already exist in the system.
                throw new Exception($"The game '{nameOfGame}' already exists");
            }
            // Generate a maze with the given size.
            IMazeGenerator mazeGenerator = new DFSMazeGenerator();
            Maze maze = mazeGenerator.Generate(rows, cols);
            maze.Name = nameOfGame;
            SPGames.Add(nameOfGame, new SinglePlayerGame(maze));
            return maze;
        }

        /// <summary>
        /// Return the maze-info of the specified game.
        /// </summary>
        /// <param name="nameOfGame">Name of game.</param>
        /// <returns>MazeInfo, null if the game is not exist.</returns>
        private MazeInfo GetMazeInfoOf(string nameOfGame)
        {
            MazeInfo mazeInfo = null;
            // Search in all dictionaries.
            if (SPGames.ContainsKey(nameOfGame))
            {
                mazeInfo = SPGames[nameOfGame].MazeInfo;
            }
            else if (availablesMPGames.ContainsKey(nameOfGame))
            {
                mazeInfo = availablesMPGames[nameOfGame].MazeInfo;
            }
            else if (unAvailablesMPGames.ContainsKey(nameOfGame))
            {
                mazeInfo = unAvailablesMPGames[nameOfGame].MazeInfo;
            }
            return mazeInfo;
        }

        public Solution<Position> SolveMaze(string nameOfGame, int algorithm)
        {
            MazeInfo mazeInfo = GetMazeInfoOf(nameOfGame);
            if (mazeInfo == null)
            {
                throw new Exception($"There is no game with the name '{nameOfGame}'");
            }
            if (mazeInfo.Solution == null)
            {
                // Solution is not inside the cache, so create one.
                ISearchable<Position> searchableMaze = new SearchableMaze(mazeInfo.Maze);
                ISearcher<Position> searcher = SearcherFactory<Position>.Create(algorithm);
                mazeInfo.Solution = searcher.Search(searchableMaze);
            }
            return mazeInfo.Solution;
        }

        public Maze StartGame(string nameOfGame, int rows, int cols, TcpClient client)
        {
            if (GetMazeInfoOf(nameOfGame) != null)
            {
                // The game is already exist in the system.
                throw new Exception($"The game '{nameOfGame}' already exists");
            }
            // Generate a maze with the given size.
            IMazeGenerator generator = new DFSMazeGenerator();
            Maze maze = generator.Generate(rows, cols);
            maze.Name = nameOfGame;
            MultiPlayerGame mpGame = new MultiPlayerGame(maze, client, maze.InitialPos);
            // Add the game to the suitable dictionaries.
            this.availablesMPGames.Add(nameOfGame, mpGame);
            this.playerToGame.Add(client, mpGame);
            return maze;
        }

        public string[] GetAvailableGames()
        {
            return availablesMPGames.Keys.ToArray();
        }

        public Maze JoinTo(string nameOfGame, TcpClient player)
        {
            if (!this.availablesMPGames.ContainsKey(nameOfGame))
            {
                // The game is not exist in the system.
                throw new Exception($"There is no game with the name '{nameOfGame}'");
            }
            // Assign the player to the needed game.
            MultiPlayerGame game = availablesMPGames[nameOfGame];
            Maze maze = game.Maze;
            game.Guest = new PlayerInfo(player, maze.InitialPos);
            // Remove the game from the available games and add it to the unavailable games.
            availablesMPGames.Remove(nameOfGame);
            unAvailablesMPGames.Add(nameOfGame, game);
            playerToGame.Add(player, game);
            return maze;
        }

        public string Play(string direction, TcpClient player)
        {
            if (!playerToGame.ContainsKey(player))
            {
                // Player tried to move although he is not participate in any game.
                throw new Exception("Player is not in a game, need to be in a game to play");
            }
            // Find the player-info of the player.
            MultiPlayerGame game = playerToGame[player];
            PlayerInfo playerInfo = game.GetPlayer(player);
            // Update the player location.
            bool validMove = playerInfo.Move(game.Maze, direction);
            if (!validMove)
            {
                // The direction of the player was invalid.
                throw new Exception($"Invalid Direction '{direction}'");
            }
            return game.Maze.Name;
        }

        public void Close(string nameOfGame)
        {
            MultiPlayerGame game = null;
            if (unAvailablesMPGames.ContainsKey(nameOfGame))
            {
                // Close multiplayer game with 2 players.
                game = unAvailablesMPGames[nameOfGame];
                unAvailablesMPGames.Remove(nameOfGame);
                playerToGame.Remove(game.Guest.Player);
            }
            else if (availablesMPGames.ContainsKey(nameOfGame))
            {
                // Close multiplayer game with one player.
                game = availablesMPGames[nameOfGame];
                availablesMPGames.Remove(nameOfGame);
            }
            else
            {
                // The game with the given name is not exist in the system.
                throw new Exception($"There is no game with the name '{nameOfGame}'");
            }
            playerToGame.Remove(game.Host.Player);
        }

        public bool IsGameBegun(string nameOfGame)
        {
            return unAvailablesMPGames.ContainsKey(nameOfGame);
        }

        public bool IsClientInGame(TcpClient client)
        {
            return playerToGame.ContainsKey(client);
        }

        public TcpClient GetCompetitorOf(TcpClient player)
        {
            if (!playerToGame.ContainsKey(player))
            {
                // Player is not participate in any game.
                return null;
            }
            MultiPlayerGame game = this.playerToGame[player];
            return game.GetCompetitorOf(player).Player;
        }

        /// <summary>
        /// Class holds info about a maze.
        /// </summary>
        public class MazeInfo
        {
            /// <summary>
            /// Maze.
            /// </summary>
            public Maze Maze { get; set; }

            /// <summary>
            /// The maze's solution.
            /// </summary>
            public Solution<Position> Solution { get; set; }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="maze">The maze to save.</param>
            public MazeInfo(Maze maze)
            {
                this.Maze = maze;
                this.Solution = null;
            }
        }
    }
}