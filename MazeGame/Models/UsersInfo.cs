using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MazeGame.Models
{
    /* Singleton */
    public class UsersInfo
    {
        private List<User> users;

        static UsersInfo instance = null;

        private UsersInfo()
        {
            this.users = new List<User> {
                new User {Name = "Eden Shuker", Password="1234", Email="edenshuker1997@gmail.com" },
                new User {Name="Shani Shuker", Password= "5678", Email="shanina@gmail.com"}
            };
        }

        public static UsersInfo GetInstance()
        {
            if (instance == null)
            {
                instance = new UsersInfo();
            }
            return instance;
        }

        public void AddUser(User user)
        {
            this.users.Add(user);
        }

        public List<User> GetAllUsers()
        {
            return this.users;
        }

        public User GetUser(string name)
        {
            foreach (User user in users)
            {
                if(user.Name==name)
                {
                    return user;
                }
            }
            return null;
        }


    }
}