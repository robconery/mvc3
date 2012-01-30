using System.Web.Mvc;

namespace VidPub.Web.Areas.Api {
    public class ApiAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Api";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Api_default",
                "api/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
