using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace VidPub.Web.Infrastructure {
    public class FormsAuthTokenStore : ITokenHandler {

        public void SetClientAccess(string token) {
            FormsAuthentication.SetAuthCookie(token, true);
        }

        public void RemoveClientAccess() {
            FormsAuthentication.SignOut();
        }

        public string GetToken() {
            var cookieName = FormsAuthentication.FormsCookieName;
            var cookieValue = HttpContext.Current.Response.Cookies[cookieName].Value;
            return cookieValue == null ? "" :   FormsAuthentication.Decrypt(cookieValue).Name;
        }

    }
    public interface ITokenHandler {
        string GetToken();
        void RemoveClientAccess();
        void SetClientAccess(string token);
    }
}