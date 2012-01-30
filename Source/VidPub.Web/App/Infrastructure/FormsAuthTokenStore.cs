using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;

namespace VidPub.Web.Infrastructure {

    public class FormsAuthTokenStore : ITokenHandler {

        public void SetClientAccess(string token) {
            HttpContext.Current.Response.Cookies["auth"].Value = token;
            HttpContext.Current.Response.Cookies["auth"].Expires = DateTime.Today.AddDays(60);
            HttpContext.Current.Response.Cookies["auth"].HttpOnly = true;

            //play nicely with FormsAuth
            FormsAuthentication.SetAuthCookie(token, true);
        }

        public void RemoveClientAccess() {
            FormsAuthentication.SignOut();
        }

        public string GetToken() {
            var result = "";
            if (HttpContext.Current.Request.Cookies["auth"] != null) {
                result = HttpContext.Current.Request.Cookies["auth"].Value;
            }
            return result;
        }

    }
    public interface ITokenHandler {
        string GetToken();
        void RemoveClientAccess();
        void SetClientAccess(string token);
    }
}