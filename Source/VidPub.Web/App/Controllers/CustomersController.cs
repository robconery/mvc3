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

        public override ActionResult Edit(int id, FormCollection collection) {
            var model = _table.CreateFrom(collection);
            try {
                // TODO: Add update logic here
                _table.Update(model, id);
                TempData["message"] = "Successfully updated " + model.Email;
                return RedirectToAction("customers", "vidpub", new { id = id });
            } catch (Exception x) {
                TempData["error"] = "There was a problem editing this record";
                return View(model);
            }
        }
    }
}

