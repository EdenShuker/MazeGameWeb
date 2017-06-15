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
            JObject obj = JObject.Parse(ToJson(maze));
            return obj;
        }



        private string ToJson(Maze maze)
        {
            JObject mazeObj = new JObject();
            mazeObj["Name"] = maze.Name;
            mazeObj["Maze"] = this.ToStringWithoutSpace(maze);
            mazeObj["Rows"] = maze.Rows;
            mazeObj["Cols"] = maze.Cols;

            JObject startObj = new JObject();
            startObj["Row"] = maze.InitialPos.Row;
            startObj["Col"] = maze.InitialPos.Col;
            mazeObj["Start"] = startObj;

            JObject endObj = new JObject();
            endObj["Row"] = maze.GoalPos.Row;
            endObj["Col"] = maze.GoalPos.Col;
            mazeObj["End"] = endObj;

            return mazeObj.ToString();
        }


        private string ToStringWithoutSpace(Maze maze)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < maze.Rows; i++)
            {
                for (int j = 0; j < maze.Cols; j++)
                {
                    sb.Append((int)maze[i, j]);
                }
            }
            return sb.ToString();
        }
    }
}
