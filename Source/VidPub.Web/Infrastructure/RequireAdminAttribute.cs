using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Controllers;

namespace VidPub.Web.Infrastructure {
    public class RequireAdminAttribute:ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext filterContext) {

            var controller = (ApplicationController)filterContext.Controller;

            //user logged in?
            if (!controller.IsLoggedIn) {
                controller.TempData["Error"] = "You need to be logged in to do that";
                filterContext.HttpContext.Response.Redirect("/account/logon");
                return;
            }

            //is the user an admin?
            var adminEmails = new string[] { "robconery@gmail.com", "rob@tekpub.com" };
            string userEmail = controller.CurrentUser.Email;
            if (!adminEmails.Contains(userEmail)) {
                controller.TempData["Error"] = "You're not authorized yo...";
                filterContext.HttpContext.Response.Redirect("/account/logon");
            }
        }
    }
}