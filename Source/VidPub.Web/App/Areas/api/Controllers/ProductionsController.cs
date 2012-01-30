using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
using VidPub.Web.Controllers;
using Massive;
using System.IO;
using System.Web.Script.Serialization;
using System.Dynamic;
namespace VidPub.Web.Areas.Api.Controllers{
    public class ProductionsController : ApplicationController {

        dynamic _productions;
        public ProductionsController(ITokenHandler tokenStore):base(tokenStore) {
            _productions = new Productions();
        }
        [HttpGet]
        public ActionResult Index(string query="") {
            if (string.IsNullOrEmpty(query)) {
                return VidpubJSON(_productions.All());
            } else {
                return VidpubJSON(_productions.FuzzySearch(query));
            }
        }

        [HttpPut]
        public ActionResult Edit() {
            var model = SqueezeJson();
            _productions.Update(model, model.ID);
            return VidpubJSON(model);
        }

        [HttpPost]
        public ActionResult Create() {
            var model = SqueezeJson();
            _productions.Insert(model);
            return VidpubJSON(model);
        }

    }

}

