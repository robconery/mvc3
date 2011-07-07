using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
namespace VidPub.Web.Controllers{
    public class EpisodesController : ApplicationController {
        dynamic _table;
        public EpisodesController(ITokenHandler tokenStore):base(tokenStore) {
            _table = new Episodes();
            ViewBag.Table = _table;
        }
        public ActionResult Index() {
            return View(_table.All());
        }
        public ActionResult Details(int id) {
            return View(_table.FindBy(ID: id, schema: true));
        }

        public ActionResult Create() {
            return View(_table.Prototype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection collection) {
            var model = _table.CreateFrom(collection);
            try {
                // TODO: Add insert logic here
                _table.Insert(model);
                return RedirectToAction("Index");
            } catch (Exception x) {
                TempData["Error"] = "There was a problem adding this record";
                return View();
            }
        }

        public ActionResult Edit(int id) {
            var model = _table.Get(ID: id);
            model._Table = _table;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection collection) {
            var model = _table.CreateFrom(collection);
            try {
                // TODO: Add update logic here
                _table.Update(model, id);
                return RedirectToAction("Index");
            } catch (Exception x) {
                TempData["Error"] = "There was a problem editing this record";
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id) {
            try {
                // TODO: Add delete logic here
                _table.Delete(id);
                return RedirectToAction("Index");
            } catch {
                TempData["Error"] = "There was a problem deleting this record";
            }
            return RedirectToAction("Index");
        }
    }
}

