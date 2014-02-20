using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using IDSM.Model;
using IDSM.ServiceLayer;
using Moq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace IDSM.Tests.Services
{
    [TestFixture]
    public class ServiceLayerTest
    {
        Mock<IService> _mockServiceLayer;

        public ServiceLayerTest()
        {
            // autofixture automatically creates objects
            _mockServiceLayer = new Mock<IService>();
        }

        [Test]
        public void CreateaGame_saves_a_game_via_context()
        {
            //var mockSet = new Mock<DbSet<Blog>>();

            //var mockContext = new Mock<BloggingContext>();
            //mockContext.Setup(m => m.Blogs).Returns(mockSet.Object);

            //_mockServiceLayer = new Mock<IService>();
            //_mockServiceLayer.Setup(s=>s.CreateGame(1, "Test Game").Status).Returns(true);

            //mockSet.Verify(m => m.Add(It.IsAny<Blog>()), Times.Once());
            //mockContext.Verify(m => m.SaveChanges(), Times.Once());
        } 

        //[Test]
        //public void GetAllChosenUserTeamPlayerIdsForGame(int gameId)
        //{
        //    //Arrange - done in constructor.
            
        //    //Act
        //    //Action

        //    //Assert
        //    Assert.IsInstanceOf<ViewResult>(result);

        //    var _chosenPlayers = _userTeamPlayers.GetList(p => p.GameId == gameId, p => p.PlayerId).Select(x => x.PlayerId).ToArray();
        //    return _chosenPlayers;
        //}
    }
}
