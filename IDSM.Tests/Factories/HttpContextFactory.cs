using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace IDSM.Tests.Factories
{
    public static class HttpContextFactory
    {
        public static void SetFakeAuthenticatedControllerContext(this Controller controller)
        {

            var httpContext = FakeAuthenticatedHttpContext();
            ControllerContext context =
            new ControllerContext(
            new RequestContext(httpContext,
            new RouteData()), controller);
            controller.ControllerContext = context;

        }


        private static HttpContextBase FakeAuthenticatedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new Mock<IPrincipal>();
            var identity = new Mock<IIdentity>();

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("jamie_admin");

            return context.Object;
        }


    }
}

