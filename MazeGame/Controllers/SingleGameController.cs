using System;
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

        [HttpGet]
        [Route("api/SingleGame/GenerateMaze/{name}/{rows}/{cols}")]
        public JObject GenerateMaze(string name, int rows, int cols)
        {
            Maze maze = ServerModel.GetInstance().GenerateMaze(name, rows, cols);
            JObject obj = JObject.Parse(maze.ToJSON());
            return obj;
        }

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

        /*[HttpGet]
        [Route("api/SingleGame/Example/{paramOne}/{paramTwo}")]
        public string Get(int paramOne, int paramTwo)
        {
            return "The [Route] with multiple params worked";
        }*/
    }
}
