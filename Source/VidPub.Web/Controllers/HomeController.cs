using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Infrastructure.Logging;

namespace VidPub.Web.Controllers {
    public class HomeController : Controller {
        ILogger _logger;
        public HomeController(ILogger logger) {
            _logger = logger;
        }
        
        public ActionResult Index() {
            _logger.LogInfo("Hey - I called the Home page!");
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About() {
            return View();
        }
    }
}
