using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VidPub.Web.Model;

namespace VidPub.Tests.Reports {
    [TestFixture]
    public class OrderReportingSpecs:TestBase {

        dynamic _items;

        [SetUp]
        public void Init() {
            _items = new OrderItems();
        }

        [Test]
        public void order_item_should_have_discount() {
            var item = _items.First();
            Assert.AreEqual(0, item.Discount);
        }
        [Test]
        public void order_item_should_not_have_productid() {
            var item = _items.First();
            var dc = (IDictionary<string, object>)item;
            Assert.False(dc.Keys.Any(x => x == "ProductID"));
        }
        [Test]
        public void order_item_should_have_required_fields() {
            var item = _items.First();
            var dc = (IDictionary<string, object>)item;
            Assert.True(dc.Keys.Any(x => x == "Price"));
            Assert.True(dc.Keys.Any(x => x == "Name"));
            Assert.True(dc.Keys.Any(x => x == "SKU"));
            Assert.True(dc.Keys.Any(x => x == "Quantity"));
            Assert.True(dc.Keys.Any(x => x == "Vendor"));
            Assert.True(dc.Keys.Any(x => x == "Discount"));
            Assert.True(dc.Keys.Any(x => x == "OrderType"));
        }

    }
}
