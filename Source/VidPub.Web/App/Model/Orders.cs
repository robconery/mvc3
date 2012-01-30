using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Orders:DynamicModel {
        public Orders():base("Vidpub","Orders","ID","OrderNumber") {
            
        }

        public dynamic CreateFromPing(dynamic order){
            //this is sorta ugly... but whatever...
            int newOrderID = 0;
            var orderItems = new OrderItems();
            
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

            try {
                dynamic savedOrder = Insert(newOrder);
                newOrderID = savedOrder.ID;
                var items = new List<dynamic>();
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
                    items.Add(newItem);
                }

                //batch add the line items
                orderItems.Save(items);

            } catch (Exception x) {
                
                //back it out
                orderItems.Delete(where: "where orderID=@0");
                Delete(newOrderID);

                //rethrow and let bubble
                throw x;
            }
            return newOrder;
        }
    }
    public class OrderItems : DynamicModel {
        public OrderItems()
            : base("Vidpub", "OrderItems", "ID", "Name") {

        }
        public override bool BeforeSave(dynamic item) {

            if (item.SKU == "monthly" || item.SKU == "yearly") {
                item.OrderType = "subscription";
            } else {
                item.OrderType = "single";
                //here is where you would assign the channel
                //or set a trigger in your DB
            }


            return true;
        }
    }
}