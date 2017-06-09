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
        private static List<User> users = new List<User>
        {
            new User {Name = "Eden Shuker", Password="1234", Email="edenshuker1997@gmail.com" },
            new User {Name="Shani Shuker", Password= "5678", Email="shanina@gmail.com"}
        };

        public IEnumerable<User> GetAllUsers()
        {
            return users;
        }

        [HttpPost]
        public void AddUser(User user)
        {
            users.Add(user);
        }
    }
}
