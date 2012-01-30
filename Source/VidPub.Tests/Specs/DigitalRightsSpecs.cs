using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VidPub.Web.Model;
using NUnit.Framework;
using Massive;

namespace VidPub.Tests.Specs {
    [TestFixture]
    public class DigitalRightsSpecs:TestBase {

        dynamic _productions;
        dynamic _episodes;
        dynamic _customers;
        dynamic _orders;
        dynamic _orderItems;

        dynamic production;
        dynamic customer;
        dynamic order;
        [SetUp]
        public void Init() {
            _productions = new Productions();
            _episodes = new Episodes();
            _customers = new Customers();
            _orders = new Orders();
            _orderItems = new OrderItems();

            _orderItems.Delete();
            _orders.Delete();

            _episodes.Delete();
            Massive.DB.Current.Execute("DELETE FROM Customers_Productions");
            _productions.Delete();
            _customers.Delete();
            
            //seeds
            production = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00, Status = "released" });
            customer = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User" });
            order = _orders.Insert(new { OrderNumber = "1", ShopifyID = 1, ShopifyName = "test", Subtotal = 100, Total = 100, Email = "test@test.com" });
        }

        dynamic MonthlyOrder() {
            return _orderItems.Insert(new { OrderID = order.ID, SKU = "monthly", Price = "30", Title= "Monthly Sub" });
        }
        dynamic AnnualOrder() {
            return _orderItems.Insert(new { OrderID = order.ID, SKU = "yearly", Price = "279", Title = "Yearly Sub" });
        }
        dynamic ProductOrder() {
            return _orderItems.Insert(new { OrderID = order.ID, SKU = production.Slug, Price = production.Price, Title = "Mastering Something" });
        }
        void AssignRights() {
            DigitalRights.Authorize(customer, production);
        }

        [Test]
        public void customer_can_stream_if_stream_date_valid() {
            customer.StreamUntil = DateTime.Today.AddDays(30);
            Assert.True(DigitalRights.CanStream(customer, production));
        }
        [Test]
        public void customer_cannot_stream_if_stream_date_invalid() {
            customer.StreamUntil = DateTime.Today.AddDays(-30);
            Assert.False(DigitalRights.CanStream(customer, production));
        }
        [Test]
        public void customer_can_download_if_stream_date_valid() {
            customer.DownloadUntil = DateTime.Today.AddDays(30);
            Assert.True(DigitalRights.CanDownload(customer, production));
        }
        [Test]
        public void customer_cannot_download_if_stream_date_invalid() {
            customer.DownloadUntil = DateTime.Today.AddDays(-30);
            Assert.False(DigitalRights.CanDownload(customer, production));
        }
        [Test]
        public void customer_can_download_and_stream_if_production_authorized() {
            AssignRights();
            Assert.True(DigitalRights.CanDownload(customer, production));
            Assert.True(DigitalRights.CanStream(customer, production));
        }
        [Test]
        public void customer_cant_download_and_stream_if_production_revoked() {
            AssignRights();
            Assert.True(DigitalRights.CanDownload(customer, production));
            Assert.True(DigitalRights.CanStream(customer, production));
            DigitalRights.Revoke(customer, production);
            Assert.False(DigitalRights.CanDownload(customer, production));
            Assert.False(DigitalRights.CanStream(customer, production));
        }

        [Test]
        public void monthly_order_should_bump_users_stream() {
            customer.StreamUntil = DateTime.Today.AddYears(-1);
            Assert.False(DigitalRights.CanStream(customer, production));
            MonthlyOrder();
            DigitalRights.AuthorizeOrder(customer, order);
            Assert.Greater(DateTime.Today.AddDays(25), customer.StreamUntil);
            Assert.True(DigitalRights.CanStream(customer, production));
        }
        [Test]
        public void yearly_order_should_bump_users_stream() {
            //reset
            customer.StreamUntil = DateTime.Today.AddYears(-2);
            customer.DownloadUntil = DateTime.Today.AddYears(-2);
            Assert.False(DigitalRights.CanDownload(customer, production));
            Assert.False(DigitalRights.CanStream(customer, production));
            AnnualOrder();
            DigitalRights.AuthorizeOrder(customer, order);
            Assert.Greater(DateTime.Today.AddMonths(11),customer.StreamUntil);
            Assert.Greater(DateTime.Today.AddMonths(11),customer.DownloadUntil);
            Assert.True(DigitalRights.CanDownload(customer, production));
            Assert.True(DigitalRights.CanStream(customer, production));
        }
        [Test]
        public void production_order_should_allow_access_forever() {
            //reset
            Massive.DB.Current.Execute("DELETE FROM Customers_Productions");
            Assert.False(DigitalRights.CanDownload(customer, production));
            Assert.False(DigitalRights.CanStream(customer, production));
            ProductOrder();
            DigitalRights.AuthorizeOrder(customer, order);
            Assert.True(DigitalRights.CanDownload(customer,production));
            Assert.True(DigitalRights.CanStream(customer,production));
        }
    }
}
