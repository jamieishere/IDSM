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
using Ploeh.AutoFixture;
using IDSM.Logging.Services.Logging.Log4Net;
using IDSM.Logging.Services.Logging;

namespace IDSM.Tests.Controllers
{
    [TestFixture]
    public class GameControllerTest
    {
        Fixture _fixture;
        UserProfile _creator;
        UserProfile _user;
        Game _game;
        ICollection<UserTeam> _uteams;
        UserTeam _ut;
        IList<UserTeam_Player> _utp;
        List<Game> _games;
        List<UserTeam> _userteams;
        Mock<IGameRepository> _mockGameRepository;
        Mock<IUserTeamRepository> _mockUserTeamRepository;
        Mock<IWebSecurityWrapper> _mockWSW;
        Mock<IUserRepository> _mockUserRepository;
        
        public GameControllerTest()
        {
            _fixture = new Fixture();
            _creator = _fixture.Create<UserProfile>();
            _user = _fixture.Create<UserProfile>();
            _game = _fixture.Create<Game>();
            _uteams = new HashSet<UserTeam>();
            _ut = _fixture.Create<UserTeam>();
            _utp = null;
            _games = _fixture.Create<List<Game>>();
            _userteams = _fixture.Create<List<UserTeam>>();

            // Mock the Players Repository using Moq
            _mockGameRepository = new Mock<IGameRepository>();
            _mockUserTeamRepository = new Mock<IUserTeamRepository>();
            _mockWSW = new Mock<IWebSecurityWrapper>();
            _mockUserRepository = new Mock<IUserRepository>();
        }

        [Test]
        public void Game_Index_Returns_ViewResult()
        {

            // Return all the Games
            _mockGameRepository.Setup(mr => mr.GetAllGames()).Returns(_games);
            _mockUserTeamRepository.Setup(mr => mr.GetAllUserTeams()).Returns(_userteams);

            //Arrange
            GameController Controller = new GameController(_mockGameRepository.Object, _mockUserTeamRepository.Object, _mockWSW.Object, _mockUserRepository.Object);

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
            //ICollection<UserTeam_Player> utp = null;
            IList<UserTeam_Player> utp = null;

            List<Game> games = new List<Game>
            {
                new Game { Id = 1, CreatorId = 1, Name = "Game1", UserTeams=uteams},
            };

            List<UserTeam> userteams = new List<UserTeam>
            {
                new UserTeam {Id=1, UserId = 1, GameId=1, UserTeam_Players=utp}
            };

            // Mock the Players Repository using Moq
            Mock<IGameRepository> mockGameRepository = new Mock<IGameRepository>();
            Mock<IUserTeamRepository> mockUserTeamRepository = new Mock<IUserTeamRepository>();
            Mock<IWebSecurityWrapper> mockWSW = new Mock<IWebSecurityWrapper>();
            Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

            // Setup 'mock' methods with dummy parameters to replace the method calls in the controller
            mockUserTeamRepository.Setup(mr => mr.GetAllUserTeams()).Returns(userteams);
            mockUserTeamRepository.Setup(mr => mr.GetUserTeam(0,1,1)).Returns(ut);
            mockUserTeamRepository.Setup(mr => mr.CreateUserTeam(1, 1));
            mockWSW.Setup(x => x.CurrentUserId).Returns(1);

            //Arrange
            GameController Controller = new GameController(mockGameRepository.Object, mockUserTeamRepository.Object, mockWSW.Object, mockUserRepository.Object);

            HttpContextFactory.SetFakeAuthenticatedControllerContext(Controller);

            //Act
            //RedirectToRouteResult result = Controller.JoinGame(1, mockWSW.Object);
            RedirectToRouteResult result = Controller.ManageUserTeam(1, 1);

            // Assert.That(result.RouteName, Is.EqualTo("Index"));
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}
