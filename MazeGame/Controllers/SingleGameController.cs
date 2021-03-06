﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeGame.Models;
using Newtonsoft.Json.Linq;
using MazeLib;
using SearchAlgorithmsLib;
using System.Text;

namespace MazeGame.Controllers
{
    public class SingleGameController : ApiController
    {
        /// <summary>
        /// Return a maze object with the given parameters.
        /// </summary>
        /// <param name="name"> name of game </param>
        /// <param name="rows">number of rows</param>
        /// <param name="cols">number of cols</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/SingleGame/GenerateMaze/{name}/{rows}/{cols}")]
        public JObject GenerateMaze(string name, int rows, int cols)
        {
            JObject obj;
            try
            {
                Maze maze = ServerModel.GetInstance().GenerateMaze(name, rows, cols);
                obj = JObject.Parse(maze.ToJSON());
            }
            catch (Exception e)
            {
                obj = new JObject {["msg"] = "Game already exists"};
            }
            return obj;
        }


        /// <summary>
        /// Return the solution of the given game.
        /// </summary>
        /// <param name="name"> name of game </param>
        /// <param name="algo"> the search algorithm </param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/SingleGame/SolveMaze/{name}/{algo}")]
        public JObject SolveMaze(string name, int algo)
        {
            Solution<Position> solution = ServerModel.GetInstance().SolveMaze(name, algo);
            return JObject.Parse(solution.ToJSON(ParseDirections));
        }

        /// <summary>
        /// Function that convert states to string-representation.
        /// </summary>
        /// <param name="path">Stack of states.</param>
        /// <returns>String representation of the path.</returns>
        static string ParseDirections(Stack<State<Position>> path)
        {
            StringBuilder builder = new StringBuilder();
            Position from = path.Pop().Data;
            while (path.Count != 0)
            {
                Position to = path.Pop().Data;
                string num;
                // Up.
                if (from.Row > to.Row)
                {
                    num = "2";
                }
                // Down.
                else if (from.Row < to.Row)
                {
                    num = "3";
                }
                // Left.
                else if (from.Col > to.Col)
                {
                    num = "0";
                }
                // Right.
                else
                {
                    num = "1";
                }
                builder.Append(num);
                // Advance to the next state.
                from = to;
            }
            return builder.ToString();
        }
    }
}