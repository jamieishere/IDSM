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
    /// <summary>
    /// IDSMContext
    /// DataContext for application.  Inherits from DbContext.
    /// </summary>
    public class IDSMContext: DbContext, IDisposedTracker
    {
        public DbSet<Banter> Banters { get; set; }
        public DbSet<FinalScore> FinalScores { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<UserProfile> UserProfiles{ get; set; }
        public DbSet<UserTeam> UserTeams { get; set; }
        //public DbSet<UsersTeams> UsersTeams { get; set; }
        public DbSet<UserTeam_Player> UserTeam_Players { get; set; }
        public bool IsDisposed { get; set; }

        /// <summary>
        /// OnModelCreating
        /// Sets rules on migrations for how EF will handle model.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Multiple cascading paths occurs where there are cyclical references
            // Eg: UserProfile -> UserTeam,       UserProfile -> Game -> UserTeam
            // See http://stackoverflow.com/questions/7367339/net-mvc-cyclical-reference-issue-with-entity

            // Previously, I used the fluent API here to prevent multiple cacade paths
            // This masked a deeper design problem with the API - I had navigation properties of child objects referring to the parent, creating circular references.
            // By removing those navigation properties, I eliminated the cyclical references, and the need for these Fluent API cascadeondelete modifications.

            modelBuilder.Entity<UserProfile>()
                    .HasMany(u => u.Games)
                    .WithRequired(i => i.Creator)
                    .HasForeignKey(i => i.CreatorId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
                    .HasMany(u => u.UserTeams)
                    .WithRequired(i => i.User)
                    .HasForeignKey(i => i.UserId)
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserTeam>()
                    .HasMany(u => u.UserTeam_Players)
                    .WithRequired(i => i.UserTeam)
                    .HasForeignKey(i => i.UserTeamId)
                    .WillCascadeOnDelete(true);

             base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}
