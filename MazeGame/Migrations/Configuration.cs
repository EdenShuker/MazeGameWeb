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
            context.Users.AddOrUpdate(
                            x => x.Name,
                            new User() { Name = "Eden", Email = "edenshuker1997@gmail.com", Password = "1234" },
                            new User() { Name = "Tamir", Email = "norblet82@gmail.com", Password = "123456" });

            context.UserRankings.AddOrUpdate(
                x => x.Name,
                new UserRankings() { Name = "Eden", Rank = 1, Losses = 0, Wins = 1 },
                new UserRankings() { Name = "Tamir", Rank = 0, Losses = 0, Wins = 0 });
        }
    }
}
