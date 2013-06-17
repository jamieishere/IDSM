using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using IDSM;
using IDSM.Controllers;
using Moq;
using IDSM.Models;
using IDSM.Repository;
using IDSM.Model;
using IDSM.Tests.Factories;
using IDSM.Wrapper;

namespace IDSM.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTest
    {
        [Test]
        public void Game_Index_Returns_ViewResult()
        {
            // create some mock players to play with
            UserProfile creator = new UserProfile();
            UserProfile user = new UserProfile();
            Game game = new Game();
            ICollection<UserTeam> uteams = null; 
            UserTeam ut = new UserTeam();
            ICollection<UserTeam_Player> utp = null;

            List<Game> games = new List<Game>
            {
                new Game { Id = 1, CreatorId = 1, Name = "Game1", Creator = creator, UserTeams=uteams},
            };

            List<UserTeam> userteams = new List<UserTeam>
            {
                new UserTeam {Id=1, UserId = 1, GameId=1, User=user, Game = game, UserTeam_Players=utp}
            };

            // Mock the Players Repository using Moq
            Mock<IGameRepository> mockGameRepository = new Mock<IGameRepository>();
            Mock<IUserTeamRepository> mockUserTeamRepository = new Mock<IUserTeamRepository>();
            Mock<IWebSecurityWrapper> mockWSW = new Mock<IWebSecurityWrapper>();

            // Return all the Games
            mockGameRepository.Setup(mr => mr.GetAllGames()).Returns(games);
            mockUserTeamRepository.Setup(mr => mr.GetAllUserTeams()).Returns(userteams);

            //Arrange
            GameController Controller = new GameController(mockGameRepository.Object, mockUserTeamRepository.Object, mockWSW.Object);

            //Act
            ViewResult result = Controller.Index();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);

        }

        [Test]
        public void Game_JoinGame_Returns_RedirectToAction()
        {
            // create some mock players to play with
            UserProfile creator = new UserProfile();
            UserProfile user = new UserProfile();
            Game game = new Game();
            ICollection<UserTeam> uteams = null;
            UserTeam ut = new UserTeam();
            ICollection<UserTeam_Player> utp = null;

            List<Game> games = new List<Game>
            {
                new Game { Id = 1, CreatorId = 1, Name = "Game1", Creator = creator, UserTeams=uteams},
            };

            List<UserTeam> userteams = new List<UserTeam>
            {
                new UserTeam {Id=1, UserId = 1, GameId=1, User=user, Game = game, UserTeam_Players=utp}
            };

            // Mock the Players Repository using Moq
            Mock<IGameRepository> mockGameRepository = new Mock<IGameRepository>();
            Mock<IUserTeamRepository> mockUserTeamRepository = new Mock<IUserTeamRepository>();
            // if i set this to Mock<IWebSecurityWrapper> mockWSW = new Mock<IWebSecurityWrapper>();
            // like the above 2, it doesn't work... (currentuserid remains 0, not 1)...
            // so how do the above 2 work sucessfully?  ah, that's it - they dont contain VALUES.
            Mock<IWebSecurityWrapper> mockWSW = new Mock<IWebSecurityWrapper>();

            // Setup 'mock' methods with dummy parameters to replace the method calls in the controller
            mockUserTeamRepository.Setup(mr => mr.GetAllUserTeams()).Returns(userteams);
            mockUserTeamRepository.Setup(mr => mr.GetUserTeam(0,1,1)).Returns(ut);
            mockUserTeamRepository.Setup(mr => mr.CreateUserTeam(1, 1));
            mockWSW.Setup(x => x.CurrentUserId).Returns(1);

            //Arrange
            GameController Controller = new GameController(mockGameRepository.Object, mockUserTeamRepository.Object, mockWSW.Object);

            //http://stackoverflow.com/questions/11245059/how-to-mock-httpcontext-so-that-it-is-not-null-from-a-unit-test
            //http://www.emadibrahim.com/2008/04/04/unit-test-linq-to-sql-in-aspnet-mvc-with-moq/
            // ohh... the httpcontextwrapper works for the old .net membership, but not for websecurity... not sure why
            // so....http://stackoverflow.com/questions/15946579/mocking-websecurity-provider
            //https://en.wikipedia.org/wiki/Adapter_pattern
            //http://forums.asp.net/t/1874799.aspx/1?What+is+the+relation+between+WebSecurity+class+and+SimpleMembershipProvider+class+and+SimpleRoleProvider+class+

            // Apparently this should set the
            HttpContextFactory.SetFakeAuthenticatedControllerContext(Controller);

            //Act
           // http://codereview.stackexchange.com/questions/15501/creating-a-wrapper-class-to-use-for-mocking-that-uses-idisposable
            // getting an error because the websecuritywrapper that i've created doesn't seem to return '1'
            //  when i replace int UserID = wr.CurrentUserId;  with int UserID = 1; in  joingame, the test passes.

            //RedirectToRouteResult result = Controller.JoinGame(1, mockWSW.Object);
            RedirectToRouteResult result = Controller.JoinGame(1);

           // Assert.That(result.RouteName, Is.EqualTo("Index"));
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
