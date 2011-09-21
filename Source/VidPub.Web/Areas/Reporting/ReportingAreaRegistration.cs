using System.Web.Mvc;

namespace VidPub.Web.Areas.Reporting {
    public class ReportingAreaRegistration : AreaRegistration {
        public override string AreaName {
            get {
                return "Reporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) {
            context.MapRoute(
                "Reporting_default",
                "Reporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
