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
using IDSM.ServiceLayer;

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
        Mock<IUserTeam_PlayerRepository> _mockUserTeamPlayerRepository;
        Mock<IWebSecurityWrapper> _mockWSW;
        Mock<IUserRepository> _mockUserRepository;
        Mock<IService> _mockServiceLayer;
        Mock<IUnitOfWork> _mockUnitOfWork;

        public GameControllerTest()
        {
            // autofixture automatically creates objects
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior(1)); //Recursion of 1
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
            _mockUserTeamPlayerRepository = new Mock<IUserTeam_PlayerRepository>();
            _mockWSW = new Mock<IWebSecurityWrapper>();
            _mockUserRepository = new Mock<IUserRepository>();

           // _mockUnitOfWork = new Mock<IUnitOfWork>();
        //    _mockServiceLayer = new Mock<IService>(_mockUnitOfWork.Object);
            _mockServiceLayer = new Mock<IService>();
            _mockServiceLayer.Setup(s => s.Users).Returns(_mockUserRepository.Object);
            _mockServiceLayer.Setup(s => s.UserTeamPlayers).Returns(_mockUserTeamPlayerRepository.Object);
            _mockServiceLayer.Setup(s => s.Games).Returns(_mockGameRepository.Object);
            _mockServiceLayer.Setup(s => s.UserTeams).Returns(_mockUserTeamRepository.Object);
        }

        [Test]
        public void Game_Index_Returns_ViewResult()
        {
            //Arrange
            //GameController Controller = new GameController(_mockGameRepository.Object, _mockUserTeamRepository.Object, _mockWSW.Object, _mockUserRepository.Object);
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ViewResult result = Controller.Index();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_Create_Returns_ViewResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ViewResult result = Controller.Create(_game);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_ViewUsers_Returns_ActionResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ViewResult result = Controller.ViewUsers(_game);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_ResetGame_Returns_ActionResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ActionResult result = Controller.ResetGame(_game.Id);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void Game_StartGame_Returns_ActionResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ActionResult result = Controller.StartGame(_game.Id);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void Game_AddUserToGame_Returns_ActionResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ActionResult result = Controller.AddUserToGame(_user.UserId, _game.Id);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void Game_ManageUserTeam_Returns_ActionResult()
        {
            //Arrange
            GameController Controller = new GameController(_mockServiceLayer.Object);

            //Act
            ActionResult result = Controller.ManageUserTeam(_game.Id, _user.UserId);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

    }

}
