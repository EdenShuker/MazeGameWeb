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

        [HttpGet]
        [Route("api/SingleGame/GenerateMaze/{name}/{rows}/{cols}")]
        public string GenerateMaze(string name, int rows, int cols)
        {
            Maze maze = ServerModel.GetInstance().GenerateMaze(name, rows, cols);
            //JObject obj = JObject.Parse(maze.ToJSON());
            return "recieved data:" + maze.Name + "," + maze.Cols + "," + maze.Rows;
        }

        [HttpGet]
        [Route("api/SingleGame/Example/{paramOne}/{paramTwo}")]
        public string Get(int paramOne, int paramTwo)
        {
            return "The [Route] with multiple params worked";
        }


    }
}
