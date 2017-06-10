using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeGame.Models;

namespace MazeGame.Controllers
{
    public class LoginController : ApiController
    {
        public IHttpActionResult GetIfUserExist(string name)
        {
            User user = UsersInfo.GetInstance().GetUser(name);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}




