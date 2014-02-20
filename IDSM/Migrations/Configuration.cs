namespace IDSM.Models
{
    using System;
    using System.Configuration;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using System.Web.Security;
    using System.Web;
    using WebMatrix.WebData;
    using IDSM.Repository;
    using IDSM.Model;
    using System.Web.Hosting;
    using IDSM.Logging.Services.Logging.Log4Net;
    using IDSM.ServiceLayer;

    /// <summary>
    /// Migrations Configuration
    /// </summary>
    /// <remarks>
    /// Required for code first migrations when change the model so that updates and auto-seeds the database.  Not sure why db stopped being updated on build when model changed... check notes.  Possibly because I moved to different accoutn model- ie NOT  .net membership. (web security)...
    /// To migrate when have updated model, use 'update-database -verbose -force(this is optional) -StartupProject IDSM'
    /// Reading:
    /// How to seed the database during migration
    ///     http://blog.longle.net/2012/09/25/seeding-users-and-roles-with-mvc4-
    ///     needed to install SQL Compact Tools to be able to add/remove DB columns
    /// Migrating to sql server 2008
    ///      http://stackoverflow.com/questions/5716668/migrate-from-sqlce-4-to-sql-server-2008
    /// </remarks>
    internal sealed class Configuration : DbMigrationsConfiguration<IDSM.Repository.IDSMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; // fixes the 'Automatic migration was not applied because it would result in data loss' error from update-database in pacakge manager.
        }

        protected override void Seed(IDSM.Repository.IDSMContext context)
        {
            WebSecurity.InitializeDatabaseConnection("IDSMContext", "UserProfile", "UserId", "UserName", true);

            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            // create a few users
            if (!WebSecurity.UserExists("jamie_admin"))
                WebSecurity.CreateUserAndAccount(
                    "jamie_admin",
                    "password");
            if (!WebSecurity.UserExists("user1"))
                WebSecurity.CreateUserAndAccount(
                    "user1",
                    "password");
            if (!WebSecurity.UserExists("user2"))
                WebSecurity.CreateUserAndAccount(
                    "user2",
                    "password");

            //assign master admin user
            if (!Roles.GetRolesForUser("jamie_admin").Contains("Administrator"))
                Roles.AddUsersToRoles(new[] { "jamie_admin" }, new[] { "Administrator" });

            // create a game
            OperationStatus test2 = new OperationStatus { Status = true };
            //GameRepository _gr = new GameRepository(new IDSMContext());
            Service _service = new Service(new UnitOfWork());
            test2 = _service.CreateGame(WebSecurity.GetUserId("jamie_admin"), "First Game");

            string fileName = ConfigurationManager.AppSettings["DataUploadFileName"];

            string path = Path.Combine(ConfigurationManager.AppSettings["AppDataUploadsPath"], fileName);
            OperationStatus test = new OperationStatus { Status = true };

            //TODO:
            //This can't handle large files.  Figure out how to do it.
            //Actually, write a console application for this... don't want user to have to sit watching web browser
            //Filter player list to only premiership clubs.
            test = Service.UploadPlayersCsv(path);
        }
    }
}
