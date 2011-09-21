using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Massive;
using VidPub.Web.Model;
using System.Dynamic;
using System.Net;

namespace VidPub.Tasks {
    class Program {

        public static void Main(params string[] args) {

            LoadOrders();
            LoadLogs();
        }
        public static void LoadLogs() {
            var vidlog = new VideoLog();
            dynamic productions = new Productions();
            dynamic episodes = new Episodes();
            dynamic orderItems = new OrderItems();
            dynamic channels = new Channels();
            var orders = new Orders();

            var rand = new Random();
            Console.WriteLine("Deleting logs...");
            vidlog.Delete();

            foreach (var order in orders.All()) {
                //pull the orderItems
                var items = orderItems.Find(OrderID: order.ID);

                //loop the items
                foreach (var item in items) {

                    var slug = item.SKU;
                    if (slug == "yearly") {
                        Console.WriteLine("Loading Productions and Episodes for Annual...");

                        //create a download log for each production and episode
                        foreach (var p in productions.All()) {
                            var eps = episodes.Find(ProductionID: p.ID);
                            foreach (var e in eps) {
                                var log = new {
                                    Slug = item.SKU,
                                    EpisodeNumber = e.Number,
                                    Email = order.Email,
                                    //the download file for the episode
                                    FileName = p.Slug + "_"+e.Number+".zip",
                                    FileSize = e.HDFileSize,
                                    //1 day lag
                                    LogDate = order.CreatedAt.AddDays(1),
                                    OrderItemID = item.ID
                                };
                                vidlog.Insert(log);
                            }
                        }
                    } else if (slug == "monthly") {
                        //create a stream log for each production and episode
                        Console.WriteLine("Loading Productions and Episodes for Monthly...");
                        foreach (var p in productions.All()) {
                            var eps = episodes.Find(ProductionID: p.ID);
                            foreach (var e in eps) {
                                var log = new {
                                    Slug = item.SKU,
                                    EpisodeNumber = e.Number,
                                    Email = order.Email,
                                    //the download file for the episode
                                    FileName = p.Slug + "_" + e.Number + ".flv",
                                    FileSize = e.StreamFileSize,
                                    //1 day lag
                                    LogDate = order.CreatedAt.AddDays(1),
                                    OrderItemID = item.ID
                               };
                                vidlog.Insert(log);
                            }
                        }
                    } else {
                        var p = productions.First(Slug:item.SKU);
                        var eps = episodes.Find(ProductionID: p.ID);
                        Console.WriteLine("Loading log for {0}...",p.Slug);
                        foreach (var e in eps) {
                            var log = new {
                                Slug = item.SKU,
                                EpisodeNumber = e.Number,
                                Email = order.Email,
                                //the download file for the episode
                                FileName = p.Slug + "_" + e.Number + ".zip",
                                FileSize = e.HDFileSize,
                                //1 day lag
                                LogDate = order.CreatedAt.AddDays(1),
                                OrderItemID = item.ID
                            };
                            vidlog.Insert(log);
                        }
                    }

                }
            }
            
        }
        public static void LoadOrders() {
            var _orders = new Orders();
            var _items = new OrderItems();
            var _customers = new Customers();
            var _productions = new Productions();
            dynamic _channels = new Channels();

            var orderID = 1001;
            var rand = new Random(100);

            Console.WriteLine("Blowing away dev data");
            _items.Delete();
            _orders.Delete();
            _customers.Delete();

            for (int i = 0; i < 1000; i++) {

                var month = rand.Next(1, 12);
                var day = rand.Next(1, 28);
                var orderDate = new DateTime(2011, month, day);

                Console.WriteLine("Adding " + i + " of 1000");
                var productID = 1;
                var price = 12.00;
                if (i > 250 && i < 500) {
                    productID = 2;
                    price = 15.00;
                } else if (i >= 500 && i < 750) {
                    productID = 3;
                    price = 18.00;
                } else if (i >= 750) {
                    productID = 4;
                    price = 10.00;
                }
                var p = _productions.Single(productID);
                var c = _channels.Single(p.ChannelID);
                var name = p.Title;
                var sku = p.Slug;
                var vendor = p.Author;
                var orderType = "single";
                //do a monthly every 5th one
                if (i % 5 == 0) {
                    name = "Monthly Subscription";
                    sku = "monthly";
                    vendor = "Tekpub";
                    price = 30.00;
                    orderType = "subscription";
                }

                //do an annual every 12th
                if (i % 12 == 0) {
                    name = "Annual Subscription";
                    sku = "yearly";
                    vendor = "Tekpub";
                    price = 279.00;
                    orderType = "subscription";
                }
                var tax = price * 0.0825;


                var newOrder = new {
                    OrderNumber = Guid.NewGuid().ToString(),
                    ShopifyID = orderID,
                    ShopifyName = "#" + orderID,
                    CreatedAt = orderDate,
                    Subtotal = price,
                    Token = Guid.NewGuid().ToString(),
                    Total = price + tax,
                    TaxIncluded = true,
                    LandingPage = "http://localhost",
                    ShopifyNumber = orderID,
                    Gateway = "bogus",
                    FulfillmentStatus = "fulfilled",
                    Currency = "USD",
                    ClosedAt = orderDate,
                    AcceptsMarketing = false,
                    Tax = tax,
                    IP = "127.0.0.1",
                    Weight = 1,
                    Email = i + "@example.com",
                };

                dynamic savedOrder = _orders.Insert(newOrder);
                //line items...
                for (int x = 1; x < 3; x++) {
                    var newItem = new {
                        OrderID = savedOrder.ID,
                        Name = name,
                        Price = price,
                        Quantity = x,
                        RequiresShipping = false,
                        Title = name,
                        Grams = 2,
                        SKU = sku,
                        Vendor = vendor,
                        OrderType = orderType,
                        Channel = c.Name
                    };
                    _items.Insert(newItem);

                    if (i % 5 == 0 || i % 12 == 0)
                        break;
                }
                Console.WriteLine("Creating Customer " + i);
                var customer = new {
                    First = "First" + i,
                    Last = "Last" + i,
                    Email = savedOrder.Email
                };
                _customers.Insert(customer);
                orderID++;
            }
        }

    }

}
