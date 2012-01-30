using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidPub.Web.Model;
using VidPub.Web.Infrastructure;
using System.Dynamic;
using System.Web.Helpers;
using System.Text;
namespace VidPub.Web.Controllers{
    public class ShopifyController : CruddyController {
        Orders _orders;
        Customers _customers;
        public ShopifyController(ITokenHandler tokenStore):base(tokenStore) {
            _orders = new Orders();
            _customers = new Customers();
        }

        public ActionResult Receiver() {
            try {
                //this is a ping from Shopify, record the order...
                var json = this.ReadJson();
                var newOrder = System.Web.Helpers.Json.Decode(json);
                dynamic order = _orders.CreateFromPing(newOrder);

                //tell Customers we just received an Order
                var customer = _customers.OrderReived(order);

                //Give them rights to the bits...
                DigitalRights.AuthorizeOrder(customer, order);
            } catch (Exception x) {
                Logger.LogFatal(x);

                //rethrow
                throw x;
            }
            //if all worked out, return a 200
            //if something failed we've logged it - and Shopify will keep sending until we return 200...
            return Content("OK");
        }

    }
}

