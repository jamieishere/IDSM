using IDSM.Model;
using IDSM;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.Models;

namespace IDSM.Repository
{
    public class IDSMContext: DbContext, IDisposedTracker
    {
        public DbSet<Banter> Banters { get; set; }
        public DbSet<FinalScore> FinalScores { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles{ get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        public DbSet<UserTeam_Player> UserTeam_Players { get; set; }
        public bool IsDisposed { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           //modelBuilder.Entity<Player>().ToTable("FootballPlayers");

//            OK.. 
//so all these recent errors (Model compatibility cannot be checked because the database does not contain model metadata. Model compatibility can only be checked for databases created using Code First or Code First Migrations.)
//- caused by the fact that I extensively edited my model, VS cocked things up internally and it couldnt update
//- so i changed
//Database.SetInitializer<IDSMContext>(new DropCreateDatabaseIfModelChanges<IDSMContext>());
//To
//Database.SetInitializer<IDSMContext>(new DropCreateDatabaseAlways<IDSMContext>());

//Then I get this error
//The referential relationship will result in a cyclical reference that is not allowed. [ Constraint name = FK_dbo.UserTeams_dbo.Games_GameId ]

//This was due to multiple cascade paths for user-game-userteam
//http://stackoverflow.com/questions/7367339/net-mvc-cyclical-reference-issue-with-entity

//So i used the fluent API to add this :::  which modifys the EF interpretation of my model.

            //modelBuilder.Entity<User>()
                    modelBuilder.Entity<UserProfile>()
                    .HasMany(u => u.Games)
                    .WithRequired(i => i.Creator)
                    .HasForeignKey(i => i.CreatorId)
                    .WillCascadeOnDelete(false);

                    modelBuilder.Entity<UserTeam>()
                            .HasMany(u => u.UserTeam_Players)
                            .WithRequired(i => i.UserTeam)
                            .HasForeignKey(i => i.UserTeamId)
                            .WillCascadeOnDelete(true);


                    // Remove the logging classes, as we already have a database for these
                    //modelBuilder.Ignore<LogEvent>();

             base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
