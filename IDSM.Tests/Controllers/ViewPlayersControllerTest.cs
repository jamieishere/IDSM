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
    public class ViewPlayersControllerTest
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
        Mock<IPlayerRepository> _mockPlayerRepository;
        Player _player;

        public ViewPlayersControllerTest()
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
            _player = _fixture.Create<Player>();
            

            // Mock the Players Repository using Moq
            _mockGameRepository = new Mock<IGameRepository>();
            _mockUserTeamRepository = new Mock<IUserTeamRepository>();
            _mockWSW = new Mock<IWebSecurityWrapper>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockPlayerRepository = new Mock<IPlayerRepository>();
        }

        [Test]
        public void ViewPlayers_Index_Returns_ActionResult()
        {
            //Arrange

            ViewPlayersController Controller = new ViewPlayersController(_mockUserRepository.Object, _mockPlayerRepository.Object, _mockGameRepository.Object, _mockUserTeamRepository.Object);

            //Act
            ActionResult result = Controller.Index(_ut.Id, "", "");

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        [Test]
        public void ViewPlayers_AddPlayer_Returns_ActionResult()
        {
            //Arrange

            ViewPlayersController Controller = new ViewPlayersController(_mockUserRepository.Object, _mockPlayerRepository.Object, _mockGameRepository.Object, _mockUserTeamRepository.Object);

            //Act
            ActionResult result = Controller.AddPlayer(_player.Id, _ut.Id, _game.Id);

            //Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

      

    }

}
