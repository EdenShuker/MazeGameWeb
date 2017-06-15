namespace MazeGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserRankings",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Rank = c.Int(nullable: false),
                        Wins = c.Int(nullable: false),
                        Losses = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.UserRankings");
        }
    }
}
