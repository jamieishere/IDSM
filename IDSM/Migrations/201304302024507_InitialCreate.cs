namespace IDSM.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {

            CreateTable(
              "dbo.UserProfile",
              c => new
              {
                  UserId = c.Int(nullable: false, identity: true),
                  UserName = c.String(maxLength: 4000),
              })
              .PrimaryKey(t => t.UserId);
            

            CreateTable(
                "dbo.Banters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ParentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        BanterText = c.String(maxLength: 4000),
                        Votes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FinalScores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GameId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        UserTeamId = c.Int(nullable: false),
                        GodScorePosition = c.Int(nullable: false),
                        PlayerScorePosition = c.Int(nullable: false),
                        BanterScore = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatorId = c.Int(nullable: false),
                        Name = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.CreatorId)
                .Index(t => t.CreatorId);
            
          
            CreateTable(
                "dbo.UserTeams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserProfile", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.Games", t => t.GameId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.GameId);
            
            CreateTable(
                "dbo.UserTeam_Player",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserTeamId = c.Int(nullable: false),
                        GameId = c.Int(nullable: false),
                        PlayerId = c.Int(nullable: false),
                        PixelPosX = c.Int(nullable: false),
                        PixelPosY = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserTeams", t => t.UserTeamId, cascadeDelete: true)
                .Index(t => t.UserTeamId);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 4000),
                        Nation = c.String(maxLength: 4000),
                        Club = c.String(maxLength: 4000),
                        Position = c.String(maxLength: 4000),
                        Age = c.Int(nullable: false),
                        Acceleration = c.Int(nullable: false),
                        Aggression = c.Int(nullable: false),
                        Agility = c.Int(nullable: false),
                        Anticipation = c.Int(nullable: false),
                        Balance = c.Int(nullable: false),
                        Bravery = c.Int(nullable: false),
                        Composure = c.Int(nullable: false),
                        Concentration = c.Int(nullable: false),
                        Corners = c.Int(nullable: false),
                        Creativity = c.Int(nullable: false),
                        Crossing = c.Int(nullable: false),
                        Decisions = c.Int(nullable: false),
                        Determination = c.Int(nullable: false),
                        Dribbling = c.Int(nullable: false),
                        Finishing = c.Int(nullable: false),
                        FirstTouch = c.Int(nullable: false),
                        Flair = c.Int(nullable: false),
                        Heading = c.Int(nullable: false),
                        Influence = c.Int(nullable: false),
                        Jumping = c.Int(nullable: false),
                        LongShots = c.Int(nullable: false),
                        LongThrows = c.Int(nullable: false),
                        Marking = c.Int(nullable: false),
                        NaturalFitness = c.Int(nullable: false),
                        OffTheBall = c.Int(nullable: false),
                        Pace = c.Int(nullable: false),
                        Passing = c.Int(nullable: false),
                        Penalties = c.Int(nullable: false),
                        Positioning = c.Int(nullable: false),
                        FreeKicks = c.Int(nullable: false),
                        Stamina = c.Int(nullable: false),
                        Strength = c.Int(nullable: false),
                        Tackling = c.Int(nullable: false),
                        Teamwork = c.Int(nullable: false),
                        Technique = c.Int(nullable: false),
                        WorkRate = c.Int(nullable: false),
                        RightFoot = c.Int(nullable: false),
                        LeftFoot = c.Int(nullable: false),
                        GK = c.Int(nullable: false),
                        SW = c.Int(nullable: false),
                        DR = c.Int(nullable: false),
                        DC = c.Int(nullable: false),
                        DL = c.Int(nullable: false),
                        WBR = c.Int(nullable: false),
                        WBL = c.Int(nullable: false),
                        DM = c.Int(nullable: false),
                        MR = c.Int(nullable: false),
                        MC = c.Int(nullable: false),
                        ML = c.Int(nullable: false),
                        AMR = c.Int(nullable: false),
                        AMC = c.Int(nullable: false),
                        AML = c.Int(nullable: false),
                        ST = c.Int(nullable: false),
                        FR = c.Int(nullable: false),
                        CurA = c.Int(nullable: false),
                        Height = c.Int(nullable: false),
                        Weight = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.UserTeam_Player", new[] { "UserTeamId" });
            DropIndex("dbo.UserTeams", new[] { "GameId" });
            DropIndex("dbo.UserTeams", new[] { "UserId" });
            DropIndex("dbo.Games", new[] { "CreatorId" });
            DropForeignKey("dbo.UserTeam_Player", "UserTeamId", "dbo.UserTeams");
            DropForeignKey("dbo.UserTeams", "GameId", "dbo.Games");
            DropForeignKey("dbo.UserTeams", "UserId", "dbo.UserProfile");
            DropForeignKey("dbo.Games", "CreatorId", "dbo.UserProfile");
            DropTable("dbo.Players");
            DropTable("dbo.UserTeam_Player");
            DropTable("dbo.UserTeams");
            DropTable("dbo.UserProfile");
            DropTable("dbo.Games");
            DropTable("dbo.FinalScores");
            DropTable("dbo.Banters");
        }
    }
}
