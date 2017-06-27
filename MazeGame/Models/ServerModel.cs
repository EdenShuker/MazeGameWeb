using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeModel;
using MazeLib;
using SearchAlgorithmsLib;

namespace MazeGame.Models
{
    /* Singleton - adapter to model*/
    public class ServerModel
    {
        private static ServerModel instance = null;
        private IModel model;

        /// <summary>
        /// Constructor - create the model.
        /// </summary>
        private ServerModel()
        {
            model = new Model();
        }

        /// <summary>
        /// statuc methis returns the singke instance
        /// </summary>
        /// <returns> the instance of this class </returns>
        public static ServerModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ServerModel();
            }
            return instance;
        }

        /// <summary>
        /// Calls model generate maze method.
        /// </summary>
        /// <param name="name"> name of game </param>
        /// <param name="rows">number of rows</param>
        /// <param name="cols">number of cols</param>
        /// <returns>Maze object</returns>
        public Maze GenerateMaze(string name, int rows, int cols)
        {
            return this.model.GenerateMaze(name, rows, cols);
        }

        /// <summary>
        /// calls solve maze method of model.
        /// </summary>
        /// <param name="nameOfGame"> name of game</param>
        /// <param name="algorithm"> the search algorithm</param>
        /// <returns></returns>
        public Solution<Position> SolveMaze(string nameOfGame, int algorithm)
        {
            return this.model.SolveMaze(nameOfGame, algorithm);
        }


        /// <summary>
        /// calls start gane method of model.
        /// </summary>
        /// <param name="nameOfGame">name of game</param>
        /// <param name="rows">number of rows</param>
        /// <param name="cols">number of cols</param>
        /// <param name="playerId">id of player</param>
        /// <returns></returns>
        public Maze StartGame(string nameOfGame, int rows, int cols, string playerId)
        {
            return this.model.StartGame(nameOfGame, rows, cols, playerId);
        }

        /// <summary>
        /// calls get available games of model.
        /// </summary>
        /// <returns> string array of games </returns>
        public string[] GetAvailableGames()
        {
            return this.model.GetAvailableGames();
        }


        /// <summary>
        /// calls joinTo method of model.
        /// </summary>
        /// <param name="nameOfGame">name of game</param>
        /// <param name="playerId">id of player</param>
        /// <returns></returns>
        public Maze JoinTo(string nameOfGame, string playerId)
        {
            return this.model.JoinTo(nameOfGame, playerId);
        }


        /// <summary>
        /// calls play method of model. 
        /// </summary>
        /// <param name="direction">the direction of movement</param>
        /// <param name="playerId"> id of player</param>
        /// <returns></returns>
        public string Play(string direction, string playerId)
        {
            return this.model.Play(direction, playerId);
        }


        /// <summary>
        /// calss close game method of model.
        /// </summary>
        /// <param name="nameOfGame">name of game</param>
        public void Close(string nameOfGame)
        {
            this.model.Close(nameOfGame);
        }


        /// <summary>
        /// calls IsGameBegun method of model.
        /// </summary>
        /// <param name="nameOfGame"> name of game</param>
        /// <returns></returns>
        public bool IsGameBegun(string nameOfGame)
        {
            return this.model.IsGameBegun(nameOfGame);
        }


        /// <summary>
        /// calls IsClientInGame of model.
        /// </summary>
        /// <param name="client"></param>
        /// <returns>true if client in game</returns>
        public bool IsClientInGame(string client)
        {
            return this.model.IsClientInGame(client);
        }


        /// <summary>
        /// calls GetCompetitorOf of model.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>the id of competitor</returns>
        public string GetCompetitorOf(string playerId)
        {
            return this.model.GetCompetitorOf(playerId);
        }


        /// <summary>
        /// calls IsPlayerReachedExit of model.
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>true if player got to goal position</returns>
        public bool IsPlayerReachedExit(string playerId)
        {
            return this.model.IsPlayerReachedExit(playerId);
        }
    }
}


