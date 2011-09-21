using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web;
using Moq;
using System.Collections.Specialized;
using System.Web.Routing;
using System.Security.Principal;

namespace VidPub.Tests {
    public class TestBase {

        public void SetContext(Controller controller) {

            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();
            var user = new GenericPrincipal(new GenericIdentity("test"), new string[0]);

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            context.Setup(ctx => ctx.User).Returns(user);

            request.Setup(r => r.Cookies).Returns(new HttpCookieCollection());
            request.Setup(r => r.Form).Returns(new NameValueCollection());
            request.Setup(q => q.QueryString).Returns(new NameValueCollection());
            response.Setup(r => r.Cookies).Returns(new HttpCookieCollection());

            var rctx = new RequestContext(context.Object, new RouteData());
            controller.ControllerContext = new ControllerContext(rctx, controller);

        }


        public void Describes(string description) {
            Console.WriteLine("----------------------------------");
            Console.WriteLine(description);
            Console.WriteLine("----------------------------------");
        }
        public void IsPending() {
            Console.WriteLine(" {0} -- PENDING", GetCaller());
            Assert.Inconclusive();
        }
        public string GetCaller() {
            StackTrace stack = new StackTrace();
            return stack.GetFrame(2).GetMethod().Name.Replace("_", " ");

        }
    }
}
