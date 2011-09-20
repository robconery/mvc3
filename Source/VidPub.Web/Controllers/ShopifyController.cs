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
        OrderItems _items;
        //Highrise.Api _highrise;
        public ShopifyController(ITokenHandler tokenStore):base(tokenStore) {
            _orders = new Orders();
            _items = new OrderItems();
            //_highrise = new Highrise.Api("XYZ", "your_domain");
        }

        public ActionResult Receiver() {

            //this is a ping from Shopify, record the order...
            var json = this.ReadJson();
            var order = System.Web.Helpers.Json.Decode(json);
            //this is sorta ugly... but whatever...
            var newOrder = new {
                OrderNumber = order.order_number,
                ShopifyID = order.id,
                ShopifyName = order.name,
                Discount = order.total_discounts,
                CreatedAt = DateTime.Parse(order.created_at),
                Subtotal = order.subtotal_price,
                Token = order.token,
                Total = order.total_price,
                TaxIncluded = order.taxes_included,
                LandingPage = order.landing_site,
                ShopifyNumber = order.number,
                ReferringSite = order.referring_site,
                Note = order.note,
                Gateway = order.gateway,
                FulfillmentStatus = order.fulfillment_status,
                FinancialStatus = order.financial_status,
                Currency = order.currency,
                ClosedAt = order.closed_at,
                AcceptsMarketing = order.buyer_accepts_marketing,
                Tax = order.total_tax,
                ReferralID = order.landing_site_ref,
                IP = order.browser_ip,
                Weight = order.total_weight,
                Email = order.email
            };

            var skus = new List<string>();
            try {
                dynamic savedOrder = _orders.Insert(newOrder);
                //line items...
                foreach (var item in order.line_items) {
                    var newItem = new {
                        OrderID = savedOrder.ID,
                        ProductID = item.productID,
                        Name = item.name,
                        Price = item.price,
                        Quantity = item.quantity,
                        RequiresShipping = item.requires_shipping,
                        Title = item.title,
                        Grams = item.grams,
                        SKU = item.SKU,
                        FulfillmentStatus = item.fulfillment_status,
                        Vendor = item.vendor,
                        FulfillmentService = item.fulfillment_service

                    };
                    _items.Insert(newItem);
                    skus.Add(item.SKU);
                }

                //var newPerson = _highrise.AddPerson(order.billing_address.first_name,
                //    order.billing_address.last_name,
                //    order.email, skus.ToArray());

                //_highrise.AddNote(newPerson, "Purchased Order " + order.name);


            } catch (Exception x) {
                Logger.LogError(x);
                throw x;
            }

            return Content("OK");
        }

    }
}

