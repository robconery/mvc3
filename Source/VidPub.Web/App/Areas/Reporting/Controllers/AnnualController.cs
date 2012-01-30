using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
using VidPub.Web.Controllers;
using Massive;
namespace VidPub.Web.Areas.Reporting.Controllers{
    public class AnnualController : ApplicationController {
        public AnnualController(ITokenHandler tokenStore):base(tokenStore) {
        }

        
        public ActionResult Sales(int year = 2011) {

            var sales = DynamicModel.Open("VidPub").Query("exec Reports_AnnualSales @0", year);
            return View(sales);
            //return CSV(sales, "Vidpub_Annual_" + year + ".csv");
        }

    }
}

