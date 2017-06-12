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

        //Maze StartGame(string nameOfGame, int rows, int cols)
        //{

        //}

        string[] GetAvailableGames()
        {
            return this.model.GetAvailableGames();
        }
    }
}


