using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MazeGame.Models
{
    public class UsersInfoContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public UsersInfoContext() : base("name=UsersInfoContext")
        {
        }

        public System.Data.Entity.DbSet<MazeGame.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<MazeGame.Models.UserRankings> UserRankings { get; set; }
    }
}
