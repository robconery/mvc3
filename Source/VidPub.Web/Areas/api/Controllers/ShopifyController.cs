using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
using VidPub.Web.Controllers;
using VidPub.Web.Infrastructure.Logging;
using System.Web.Script.Serialization;
namespace VidPub.Web.Areas.api.Controllers{
    public class ShopifyController : ApplicationController {
        Orders _orders;
        public ShopifyController(ITokenHandler tokenStore, ILogger logger):base(tokenStore,logger) {
            _orders = new Orders();
        }

        //need to have an action for Shopify pings
        public ActionResult Receiver() {
            var order = this.SqueezeJSON();
            Logger.LogInfo(order.order_number.ToString());
            return Content("OK");
        }

    }
}

