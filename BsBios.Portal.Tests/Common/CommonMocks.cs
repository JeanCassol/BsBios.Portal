using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BsBios.Portal.UI;
using Moq;

namespace BsBios.Portal.Tests.Common
{
    public static class CommonMocks
    {
        public static void MockControllerUrl(Controller controller)
        {
            var routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
            request.SetupGet(x => x.ApplicationPath).Returns("/");
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost/a", UriKind.Absolute));
            request.SetupGet(x => x.ServerVariables).Returns(new System.Collections.Specialized.NameValueCollection());

            var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
            response.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>()))
                    .Returns((string path) => path);

            var context = new Mock<HttpContextBase>(MockBehavior.Strict);
            context.SetupGet(x => x.Request).Returns(request.Object);
            context.SetupGet(x => x.Response).Returns(response.Object);

            controller.Url = new UrlHelper(new RequestContext(context.Object, new RouteData()), routes);
            
        }
    }
}
