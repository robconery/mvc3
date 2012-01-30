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
                DecideResponse(filterContext.HttpContext);
                return;
            }

            //is the user an admin?
            var adminEmails = new string[] { "robconery@gmail.com", "rob@tekpub.com" };
            string userEmail = controller.CurrentUser.Email;
            if (!adminEmails.Contains(userEmail)) {
                DecideResponse(filterContext.HttpContext);
                return;
            }
            
        }
        void DecideResponse(HttpContextBase ctx) {
            if (ctx.Request.ContentType == "application/json") {
                ctx.Response.Write("Unauthorized");
            } else {
                ctx.Response.Redirect("/account/logon");
            }
            ctx.Response.End();
        }
    }
}