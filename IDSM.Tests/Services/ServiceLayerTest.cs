using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IDSM.ServiceLayer;
using Moq;
using NUnit.Framework;

namespace IDSM.Tests.Services
{
    [TestFixture]
    public class ServiceLayerTest
    {
        Mock<IService> _mockServiceLayer;

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
    }
}
