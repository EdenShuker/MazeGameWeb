using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeGame.Models;
using Newtonsoft.Json;

namespace MazeGame.Controllers
{
    public class ListGamesController : ApiController
    {

        [HttpGet]
        public JObject ListGames()
        {
            string[] games = ServerModel.GetInstance().GetAvailableGames();
            JObject obj = JObject.Parse(JsonConvert.SerializeObject(games));
            return obj;
        }
    }
}
