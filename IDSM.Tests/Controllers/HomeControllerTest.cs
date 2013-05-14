using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IDSM;
using IDSM.Controllers;
using Moq;
using IDSM.Models;
using IDSM.Repository;
using IDSM.Model;

namespace IDSM.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public HomeControllerTest()
        {
            // create some mock players to play with
            List<Player> players = new List<Player>
                {
                    new Player { Id = 1, Name = "Wayne Rooney"},
                    new Player { Id = 2, Name = "Ryan Giggs"},
                    new Player { Id = 3, Name = "Patrice Evra"}
                };

            // Mock the Players Repository using Moq
            Mock<IPlayerRepository> mockPlayerRepository = new Mock<IPlayerRepository>();

            // Return all the Players
            mockPlayerRepository.Setup(mr => mr.GetAllPlayers()).Returns(players);

            // return a Player by Id
            mockPlayerRepository.Setup(mr => mr.GetPlayer(
                It.IsAny<int>())).Returns((int i) => players.Where(
                x => x.Id == i).Single());

            // return a Player by Name
            //mockPlayerRepository.Setup(mr => mr.FindByName(
            //    It.IsAny<string>())).Returns((string s) => Players.Where(
            //    x => x.Name == s).Single());

            // Allows us to test saving a Player
            //mockPlayerRepository.Setup(mr => mr.Save(It.IsAny<Player>())).Returns(
            //    (Player target) =>
            //    {
            //        DateTime now = DateTime.Now;
 
            //        if (target.PlayerId.Equals(default(int)))
            //        {
            //            target.DateCreated = now;
            //            target.DateModified = now;
            //            target.PlayerId = Players.Count() + 1;
            //            Players.Add(target);
            //        }
            //        else
            //        {
            //            var original = Players.Where(
            //                q => q.PlayerId == target.PlayerId).Single();
 
            //            if (original == null)
            //            {
            //                return false;
            //            }
 
            //            original.Name = target.Name;
            //            original.Price = target.Price;
            //            original.Description = target.Description;
            //            original.DateModified = now;
            //        }
 
            //        return true;
            //    });
 
            // Complete the setup of our Mock Player Repository
            this.MockPlayersRepository = mockPlayerRepository.Object;
        }
 
        /// <summary>
        /// Gets or sets the test context which provides
        /// information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }
 
        /// <summary>
        /// Our Mock Players Repository for use in testing
        /// </summary>
        public readonly IPlayerRepository MockPlayersRepository;
 
        /// <summary>
        /// Can we return a Player By Id?
        /// </summary>
        [TestMethod]
        public void CanReturnPlayerById()
        {
            // Try finding a Player by id
            Player testPlayer = this.MockPlayersRepository.GetPlayer(2);
 
            Assert.IsNotNull(testPlayer); // Test if null
            Assert.IsInstanceOfType(testPlayer, typeof(Player)); // Test type
            Assert.AreEqual("Ryan Giggs", testPlayer.Name); // Verify it is the right Player
        }
 
        /// <summary>
        /// Can we return a Player By Name?
        /// </summary>
        //[TestMethod]
        //public void CanReturnPlayerByName()
        //{
        //    // Try finding a Player by Name
        //    Player testPlayer = this.MockPlayersRepository.FindByName("Silverlight Unleashed");
 
        //    Assert.IsNotNull(testPlayer); // Test if null
        //    Assert.IsInstanceOfType(testPlayer, typeof(Player)); // Test type
        //    Assert.AreEqual(3, testPlayer.PlayerId); // Verify it is the right Player
        //}
 
        /// <summary>
        /// Can we return all Players?
        /// </summary>
        [TestMethod]
        public void CanReturnAllPlayers()
        {
            // Try finding all Players
            List<Player> testPlayers = this.MockPlayersRepository.GetAllPlayers().ToList();
 
            Assert.IsNotNull(testPlayers); // Test if null
            Assert.AreEqual(3, testPlayers.Count); // Verify the correct Number
        }
 
        /// <summary>
        /// Can we insert a new Player?
        /// </summary>
        //[TestMethod]
        //public void CanInsertPlayer()
        //{
        //    // Create a new Player, not I do not supply an id
        //    Player newPlayer = new Player
        //        { Name = "Pro C#", Description = "Short description here", Price = 39.99 };
 
        //    int PlayerCount = this.MockPlayersRepository.FindAll().Count;
        //    Assert.AreEqual(3, PlayerCount); // Verify the expected Number pre-insert
 
        //    // try saving our new Player
        //    this.MockPlayersRepository.Save(newPlayer);
 
        //    // demand a recount
        //    PlayerCount = this.MockPlayersRepository.FindAll().Count;
        //    Assert.AreEqual(4, PlayerCount); // Verify the expected Number post-insert
 
        //    // verify that our new Player has been saved
        //    Player testPlayer = this.MockPlayersRepository.FindByName("Pro C#");
        //    Assert.IsNotNull(testPlayer); // Test if null
        //    Assert.IsInstanceOfType(testPlayer, typeof(Player)); // Test type
        //    Assert.AreEqual(4, testPlayer.PlayerId); // Verify it has the expected Playerid
        //}
 
        /// <summary>
        /// Can we update a prodict?
        /// </summary>
        //[TestMethod]
        //public void CanUpdatePlayer()
        //{
        //    // Find a Player by id
        //    Player testPlayer = this.MockPlayersRepository.FindById(1);
 
        //    // Change one of its properties
        //    testPlayer.Name = "C# 3.5 Unleashed";
 
        //    // Save our changes.
        //    this.MockPlayersRepository.Save(testPlayer);
 
        //    // Verify the change
        //    Assert.AreEqual("C# 3.5 Unleashed", this.MockPlayersRepository.FindById(1).Name);
        //}

        //[TestMethod]
        //public void Index_RendersView()
        //{
        //    // Arrange
        //    var controller = new HomeController(new FakePlayerRepository());

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Index_get_most_recent_entries()
        //{
        //    // Arrange
        //    var controller = new HomeController(new FakePlayerRepository());

        //    // Act
        //    ViewResult result = (ViewResult)controller.Index();

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Index_get_most_recent_entries()
        //{
        //    // Arrange
        //    var controller = new HomeController(new FakePlayerRepository());

        //    // Act
        //    ViewResult result = (ViewResult)controller.Index();

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Index()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Index() as ViewResult;

        //    // Assert
        //    Assert.AreEqual("Modify this template to jump-start your ASP.NET MVC application.", result.ViewBag.Message);
        //}

        //[TestMethod]
        //public void About()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.About() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}

        //[TestMethod]
        //public void Contact()
        //{
        //    // Arrange
        //    HomeController controller = new HomeController();

        //    // Act
        //    ViewResult result = controller.Contact() as ViewResult;

        //    // Assert
        //    Assert.IsNotNull(result);
        //}
    }
}
