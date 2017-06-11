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

namespace MazeGame.Controllers
{
    public class SingleGameController : ApiController
    {

        [HttpPost]
        [Route("api/SingleGame/GenerateMaze")]
        public JObject GenerateMaze(string name, int rows, int cols)
        {
            Maze maze = ServerModel.GetInstance().GenerateMaze(name, rows, cols);
            JObject obj = JObject.Parse(maze.ToJSON());
            return obj;
        }

        //[HttpPost]
        //[Route("api/SingleGame/SolveMaze")]        //public JObject SolveMaze(string mazeName, int algo)
        //{
        //    Solution<Position> solution = ServerModel.GetInstance().SolveMaze(mazeName, algo);
            
        //}
    }
}
