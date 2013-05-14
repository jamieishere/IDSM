using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using IDSM.Repository;
using IDSM.Model;
using System.IO;
using System.Configuration;
using WebMatrix.WebData;
using System.Web.Security;


namespace IDSM.Models
{
    // Can possibly add seeding in here, seems to work fine in the configurations.cs (migrations) file now...
    // not exactly sure of the difference between migrations and using 'dropcreatedatabasealways' / dbinitialiser...
    /// <summary>
    /// REALLY I NEED TO UNDERSTAND THIS - why use migrations over this?
    /// // I GUESS THIS (MIGRATIONS) is only valid BEFORE I HAVE FINALISED THE APPLICATION - AFTER THAT (in a production environment) it would be VERY DANGEROUS
    /// / but so would this?
    /// // CONFIGURATION / MIGRATIONS  only good useful for seeding during dev... perhsps not get used at all later...
    /// so is the configuration stuff only actually used when i use the package manager console and update-database, and THIS bdinitialiser
    /// //       class is used in the real app to initalise the database (and thus I never really want to seed anything here as in a real environment I won't actaully use it)
    /// ///I THINK
    /// // seed method here only called when actually send first sql method to DB, not in applicaiton start (where this is initialised) - so not on build, or when start in chrome, only when actually go to the players controller
    /// // the seed i use in the configurations class is different - this uses migrations to seed.
    /// // for my purposes now, the migrations method is better as i have total control (my understanding of how to use seed in this dbinitialiser may be wrong tho)
    /// </summary>
    /// 
    // where did this even come from?  can't see in acccount at a glance... or does he get rid later on?

    public class DBInitialiser : CreateDatabaseIfNotExists<IDSMContext>
    //public class DBInitialiser : DropCreateDatabaseAlways<IDSMContext> || CreateDatabaseIfNotExists || DropCreateDatabaseIfModelChanges
    {


        //protected override void Seed(IDSMContext context)
        //{
        //   // WebSecurity.InitializeDatabaseConnection("IDSMContext", "UserProfile", "UserId", "UserName", true);

        //    if (!Roles.RoleExists("Administrator"))
        //        Roles.CreateRole("Administrator");

        //    if (!WebSecurity.UserExists("jamie_admin"))
        //        WebSecurity.CreateUserAndAccount(
        //            "jamie_admin",
        //            "password");

        //    if (!Roles.GetRolesForUser("jamie_admin").Contains("Administrator"))
        //        Roles.AddUsersToRoles(new[] { "jamie_admin" }, new[] { "Administrator" });

        //    // get the UserId from the UserProfile table for jamie_admin
        //    // use this to create a record in the Games table with FK CreatorId
        //    OperationStatus test2 = new OperationStatus { Status = true };
        //    GameRepository gr = new GameRepository();
        //    test2 = gr.SaveGame(WebSecurity.GetUserId("jamie_admin"), "First Game");
        //    //throw new Exception(test2.Status.ToString());


        //    //System.Diagnostics.Debug.WriteLine(ConfigurationManager.AppSettings.Get("AppDataUploadsPath"));

        //    //string  path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("AppDataUploadsPath")), "idsm_subset_3.csv");
        //    string path = "C:/Projects/idsm_v2/IDSM/IDSM/App_Data/Uploads/idsm_subset_3.csv";
        //    OperationStatus test = new OperationStatus { Status = true };
        //    test = PlayerRepository.UploadPlayersCSV(path);



        //    //// upload the seed players csv file
        //    //string path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings.Get("AppDataUploadsPath")), "idsm_subset_3.csv");
        //    //PlayerRepository.UploadPlayersCSV(path);

        //    // now seed a user
        //    // get the id of the user
        //    // use that to seed a game

        //    //new List<Game>
        //    //{
        //    //    new Game { Id = 1, CreatorIdName = "Bob Jones" },
        //    //    new Item { Id = 2, Name = "George Smith" },
        //    //    new Item { Id = 3, Name = "Boys and Girls" },
        //    //    new Item { Id = 4, Name = "The President's Hair" },
        //    //    new Item { Id = 5, Name = "Invaders From Mars" },
        //    //    new Item { Id = 6, Name = "Tank Shooter" },
        //    //    new Item { Id = 7, Name = "Stew's Blog" },
        //    //    new Item { Id = 8, Name = "Social Mania" }
        //    //}.ForEach(i => context.Items.Add(i));

        //    //new List<Game>
        //    //{
        //    //    new Game { Id = 1, CreatorIdName = "Bob Jones" },
        //    //    new Item { Id = 2, Name = "George Smith" },
        //    //    new Item { Id = 3, Name = "Boys and Girls" },
        //    //    new Item { Id = 4, Name = "The President's Hair" },
        //    //    new Item { Id = 5, Name = "Invaders From Mars" },
        //    //    new Item { Id = 6, Name = "Tank Shooter" },
        //    //    new Item { Id = 7, Name = "Stew's Blog" },
        //    //    new Item { Id = 8, Name = "Social Mania" }
        //    //}.ForEach(i => context.Items.Add(i));

        //    //new List<Tag>
        //    //{
        //    //    new Tag { Id = 1, Name = "Author" },
        //    //    new Tag { Id = 2, Name = "Movie" },
        //    //    new Tag { Id = 3, Name = "Video Game" },
        //    //    new Tag { Id = 4, Name = "Website" }
        //    //}.ForEach(t => context.Tags.Add(t));

        //    //new List<ItemTag>
        //    //{
        //    //    new ItemTag { Id = 1, ItemId = 1, TagId = 1 },
        //    //    new ItemTag { Id = 2, ItemId = 2, TagId = 1 },
        //    //    new ItemTag { Id = 3, ItemId = 3, TagId = 2 },
        //    //    new ItemTag { Id = 4, ItemId = 4, TagId = 2 },
        //    //    new ItemTag { Id = 5, ItemId = 5, TagId = 3 },
        //    //    new ItemTag { Id = 6, ItemId = 6, TagId = 3 },
        //    //    new ItemTag { Id = 7, ItemId = 7, TagId = 4 },
        //    //    new ItemTag { Id = 8, ItemId = 8, TagId = 4 }
        //    //}.ForEach(x => context.ItemTags.Add(x));
        //}

    }
}