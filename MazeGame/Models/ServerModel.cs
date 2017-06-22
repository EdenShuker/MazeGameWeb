using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MazeModel;
using MazeLib;
using SearchAlgorithmsLib;

namespace MazeGame.Models
{
    /* Singleton */
    public class ServerModel
    {
        private static ServerModel instance = null;
        private IModel model;

        private ServerModel()
        {
            model = new Model();
        }

        public static ServerModel GetInstance()
        {
            if (instance == null)
            {
                instance = new ServerModel();
            }
            return instance;
        }

        public Maze GenerateMaze(string name, int rows, int cols)
        {
            return this.model.GenerateMaze(name, rows, cols);
        }

        public Solution<Position> SolveMaze(string nameOfGame, int algorithm)
        {
            return this.model.SolveMaze(nameOfGame, algorithm);
        }

        public Maze StartGame(string nameOfGame, int rows, int cols, string playerId)
        {
            return this.model.StartGame(nameOfGame, rows, cols, playerId);
        }

        public string[] GetAvailableGames()
        {
            return this.model.GetAvailableGames();
        }

        public Maze JoinTo(string nameOfGame, string playerId)
        {
            return this.model.JoinTo(nameOfGame, playerId);
        }

        public string Play(string direction, string playerId)
        {
            return this.model.Play(direction, playerId);
        }

        public void Close(string nameOfGame)
        {
            this.model.Close(nameOfGame);
        }

        public bool IsGameBegun(string nameOfGame)
        {
            return this.model.IsGameBegun(nameOfGame);
        }

        public bool IsClientInGame(string client)
        {
            return this.model.IsClientInGame(client);
        }

        public string GetCompetitorOf(string playerId)
        {
            return this.model.GetCompetitorOf(playerId);
        }

        public bool IsPlayerReachedExit(string playerId)
        {
            return this.model.IsPlayerReachedExit(playerId);
        }
    }
}


