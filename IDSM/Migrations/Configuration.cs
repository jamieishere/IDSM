//namespace IDSM.Repository.Migrations
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

    // I THINK THIS ONLY GETS USED FOR MIGRATIONS - THAT IS, USING THE PACKAGE MANAGER CONSOLE & update-database command.
    // (use update-database -verbose -force(this is optional) -StartupProject IDSM
    ////http://blog.longle.net/2012/09/25/seeding-users-and-roles-with-mvc4-
    // needed to install SQL Compact Tools to be able to add/remove DB columns

    // migrate to sql server 2008
    //http://stackoverflow.com/questions/5716668/migrate-from-sqlce-4-to-sql-server-2008


    internal sealed class Configuration : DbMigrationsConfiguration<IDSM.Repository.IDSMContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true; // fixes the Automatic migration was not applied because it would result in data loss error from update-database in pacakge manager.
        }

        protected override void Seed(IDSM.Repository.IDSMContext context)
        {
            // followed these instructions in an attempt to figure out how to see the database
            //http://blog.longle.net/2012/09/25/seeding-users-and-roles-with-mvc4-
            WebSecurity.InitializeDatabaseConnection("IDSMContext", "UserProfile", "UserId", "UserName", true);

            if (!Roles.RoleExists("Administrator"))
                Roles.CreateRole("Administrator");

            if (!WebSecurity.UserExists("jamie_admin"))
                WebSecurity.CreateUserAndAccount(
                    "jamie_admin",
                    "password");

            if (!Roles.GetRolesForUser("jamie_admin").Contains("Administrator"))
                Roles.AddUsersToRoles(new[] { "jamie_admin" }, new[] { "Administrator" });

            // get the UserId from the UserProfile table for jamie_admin
            // use this to create a record in the Games table with FK CreatorId
             OperationStatus test2 = new OperationStatus { Status = true };
            GameRepository gr = new GameRepository();
            test2 = gr.SaveGame(WebSecurity.GetUserId("jamie_admin"), "First Game");
            //throw new Exception(test2.Status.ToString());


            //System.Diagnostics.Debug.WriteLine(ConfigurationManager.AppSettings.Get("AppDataUploadsPath"));

            //string  path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("AppDataUploadsPath")), "idsm_subset_3.csv");
            string path = "C:/Projects/idsm_v2/IDSM/IDSM/App_Data/Uploads/idsm_subset_3.csv";
            OperationStatus test = new OperationStatus { Status = true };
            test = PlayerRepository.UploadPlayersCSV(path);
            //string filePath = HttpRuntime.AppDomainAppPath + "/App_Data/Uploads/idsm_subset_3.csv";
            //string filePath = "tetewtessaats"; //HostingEnvironment.MapPath("~/App_Data/Uploads/idsm_subset_3.csv");
            //throw new Exception(test.Status.ToString());
            //throw new Exception(test.Status.ToString());
            //throw new Exception(test.Status.ToString());
            

            // Seed a game.


            
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
