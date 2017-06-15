using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MazeGame.Models;

namespace MazeGame.Controllers
{
    public class RegisterController : ApiController
    {

        public IEnumerable<User> GetAllUsers()
        {
            return null;
        }

        [HttpPost]
        public void AddUser(User user)
        {
        }
    }
}
