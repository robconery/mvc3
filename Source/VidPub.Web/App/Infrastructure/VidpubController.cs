using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
namespace VidPub.Web.Controllers{
    public class VidpubController : ApplicationController {
        public VidpubController(ITokenHandler tokenStore):base(tokenStore) {
        }

        public ActionResult Index() {
            return View();
        }
        public ActionResult Productions() {
            return View();
        }
        public ActionResult Customers(int id) {
            var customer = new Customers().Single(id);
            return View(customer);
        }
    }
}

