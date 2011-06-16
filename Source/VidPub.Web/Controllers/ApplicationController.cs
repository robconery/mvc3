using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Infrastructure;

namespace VidPub.Web.Controllers
{
    public class ApplicationController : Controller
    {
        public ApplicationController() { }

        public ITokenHandler TokenStore;
        public ApplicationController(ITokenHandler tokenStore) {
            TokenStore = tokenStore;

            //initialize this
            ViewBag.CurrentUser = CurrentUser ?? new { Email = "" };

        }

        dynamic _currentUser;
        public dynamic CurrentUser {
            get {
                var token = TokenStore.GetToken();
                if (!String.IsNullOrEmpty(token)) {
                    _currentUser = Model.Users.FindByToken(token);

                    if (_currentUser == null) {
                        //force the current user to be logged out...
                        TokenStore.RemoveClientAccess();
                    }
                }

                //Hip to be null...
                return _currentUser;
            }

        }

        public bool IsLoggedIn {
            get {
                return CurrentUser != null;
            }
        }



    }
}
