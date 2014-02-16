using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NUnit.Framework;
using IDSM;
using IDSM.Controllers;
using Moq;
using IDSM.Models;
using IDSM.Repository;
using IDSM.Model;
using IDSM.Tests.Factories;
using System.Web;
using System.Web.Routing;
using System.Configuration;

namespace IDSM.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerTest
    {
        /// <summary>
        /// Upload
        /// Tests the posted file csv upload
        /// </summary>
        /// <remarks>
        /// It's necessary to mock HttpContextBase, don't full understand 
        /// Further reading
        /// http://stackoverflow.com/questions/5515404/how-to-unit-test-asp-net-mvc-fileupload
        /// http://stackoverflow.com/questions/8308899/unit-test-a-file-upload-how
        /// http://www.codethinked.com/simplified-aspnet-mvc-controller-testing-with-moq
        /// </remarks>
        [Test]
        public void Upload()
        {
            // Arrange
            HomeController controller = new HomeController();
            var httpContextMock = new Mock<HttpContextBase>();
            controller.ControllerContext = new ControllerContext(httpContextMock.Object, new RouteData(), controller);
            var FileUpload = new Mock<HttpPostedFileBase>();
            FileUpload.Setup(f => f.ContentLength).Returns(1);
            FileUpload.Setup(f => f.FileName).Returns("idsm_forupload.csv");

            // Act/
            ActionResult result = controller.Upload(FileUpload.Object);

            // Assert
            Assert.IsInstanceOf<ActionResult>(result);
        }

        /// <summary>
        /// Index
        /// Tests the Home page index action
        /// </summary>
        /// <remarks>
        /// Initially, this test failed because the home page text was in a resx file in the App_GlobalResources folder.  App_GlobalResources is not available in a unit test because, behind the scenes, it uses HttpContext.GetGlobalResourceObject.  I had a simlar issue with getting the UserName in the GameController test (created HttpContextFactory, didn't work, then used a WebSecurityWrapper, which did.
        /// NOTE: Apparently I don't use the websecurity wrapper anymore. Can't find it.
        /// I solve the issue (with resource files) using the approach in this link
        /// http://odetocode.com/Blogs/scott/archive/2009/07/16/resource-files-and-asp-net-mvc-projects.aspx
        ///     Change App_GlobalResources to Resources, then change the properties on the resx files to EmbeddedResource and PublicResXFileCodeGenerator.
        /// Further reading
        ///     http://stackoverflow.com/questions/4153748/app-globalresources-not-loading-in-a-unit-test-case)
        ///     http://haacked.com/archive/2007/06/19/unit-tests-web-code-without-a-web-server-using-httpsimulator.aspx
        ///     http://odetocode.com/Blogs/scott/archive/2009/07/16/resource-files-and-asp-net-mvc-projects.aspx
        /// </remarks>
        [Test]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            //ViewResult result = controller.Index() as ViewResult;
            ViewResult result = controller.Index();

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
