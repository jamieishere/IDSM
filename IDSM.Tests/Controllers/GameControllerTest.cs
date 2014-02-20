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
        Mock<IService> _mockServiceLayer;

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

            _mockServiceLayer = new Mock<IService>();
        }

        [Test]
        public void Game_Index_Returns_ViewResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);

            //Act
            ViewResult result = Controller.Index();

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_Create_Returns_ViewResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);
            var _opStatus = new OperationStatus();
            _mockServiceLayer.Setup(s => s.CreateGame(_game.CreatorId, _game.Name)).Returns(_opStatus);

            //Act
            ViewResult result = Controller.Create(_game);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_ViewUsers_Returns_ActionResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);

            //Act
            ViewResult result = Controller.ViewUsers(_game);

            //Assert
            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void Game_ResetGame_Returns_ActionResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);
            var _opStatus = new OperationStatus();
            //_mockServiceLayer.Setup(s => s.ResetGame(123)).Returns(_opStatus);
            _mockServiceLayer.Setup(s => s.ResetGame(123));

            //Act
            ActionResult result = Controller.ResetGame(123);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void Game_StartGame_Returns_ActionResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);
            var _opStatus = new OperationStatus();
            _mockServiceLayer.Setup(s => s.StartGame(123));

            //Act
            ActionResult result = Controller.StartGame(123);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void Game_AddUserToGame_Returns_ActionResult()
        {
            //Arrange
            GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);
            var _opStatus = new OperationStatus();
            _mockServiceLayer.Setup(s => s.AddUserToGame(123,123));

            //Act
            ActionResult result = Controller.AddUserToGame(123, 123);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        //[Test]
        //public void Game_ManageUserTeam_Returns_ActionResult()
        //{
        //    //Arrange
        //    GamesAdminController Controller = new GamesAdminController(_mockServiceLayer.Object);
        //    var _opStatus = new OperationStatus();
        //    UserTeam _ut = null;
        //    _mockServiceLayer.Setup(s => s.TryGetUserTeam(out _ut, 123, 123)).Returns(false);

        //    //Act
        //    RedirectToRouteResult result = Controller.ManageUserTeam(123, 123);

        //    //Assert
        //    Assert.IsInstanceOf<RedirectToRouteResult>(result);
        //}

    }

}
