using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
namespace VidPub.Web.Controllers{
    public class CustomersController : CruddyController {
        public CustomersController(ITokenHandler tokenStore):base(tokenStore) {
            _table = new Customers();
            ViewBag.Table = _table;
        }
    }
}

