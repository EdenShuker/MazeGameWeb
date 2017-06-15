namespace MazeGame.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using MazeGame.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MazeGame.Models.UsersInfoContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MazeGame.Models.UsersInfoContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.

            context.Users.AddOrUpdate(p => p.Name,
                new User() { Name = "Eden Shuker", Password = "1234", Email = "eden1997@gmail.com" },
                new User() { Name = "Tamir Moshiashvili", Password = "password", Email = "tamir1996@gmail.com" }
                );

            context.UserRankings.AddOrUpdate(p => p.Name,
                new UserRankings() { Name = "Eden Shuker", Rank = 2, Wins = 4, Losses = 2 },
                new UserRankings() { Name = "Tamir Moshiashvili", Rank = 3, Wins = 5, Losses = 2 }

                );
        }
    }
}
