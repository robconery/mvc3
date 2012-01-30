using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VidPub.Web.Model;

namespace VidPub.Tests {

    //A Production is a collection of Episodes
    //A Customer can buy a Production
    //A Customer cannot buy an individual episode

    [TestFixture]
    public class ProductionSpecs : TestBase {
        dynamic _productions;
        dynamic _episodes;
        dynamic _customers;
        [SetUp]
        public void Init() {
            _productions = new Productions();
            _episodes = new Episodes();
            _customers = new Customers();
            _episodes.Delete();
            Massive.DB.Current.Execute("DELETE FROM Customers_Productions");
            _productions.Delete();
            _customers.Delete();
        }

        [Test]
        public void a_production_has_one_or_more_episodes() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            var e = _episodes.Insert(new { Title = "Ep 1", Number = 1, ProductionID = p.ID, HDFileSize = 1, MobileFileSize = 1, TabletFileSize = 1, StreamFileSize = 1 });
            var eps = _episodes.Count(ProductionID: p.ID);
            Assert.AreEqual(1, eps);
        }

        [Test]
        public void a_production_can_cost_0_or_more_dollars() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            Assert.True(p.ID > 0);
            //negative prices... no no no
            Assert.Throws<InvalidOperationException>(delegate {
                _productions.Insert(new { Title = "Test", Slug = "Test", Price = -12.00 });
            });
        }

        [Test]
        public void a_production_can_should_be_pending_by_default() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            Assert.AreEqual("pending", p.Status);
        }
        [Test]
        public void a_production_should_not_be_downloadable_if_not_released() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            Assert.AreEqual("pending", p.Status);
        }
        [Test]
        public void a_production_is_downloadable_if_released_and_user_eligible() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00, Status="released" });
            var c = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User", DownloadUntil = DateTime.Today.AddDays(1) });
            Assert.True(DigitalRights.CanDownload(c, p));
        }
        [Test]
        public void a_production_is_streamable_if_released() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00, Status = "released" });
            var c = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User", StreamUntil=DateTime.Today.AddDays(2) });
            Assert.True(DigitalRights.CanStream(c, p));
        }
        [Test]
        public void a_production_is_not_streamable_if_not_released() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            var c = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User" });
            Assert.IsFalse(DigitalRights.CanStream(c, p));
        }

        [Test]
        public void a_production_is_not_downloadable_if_not_released() {
            var p = _productions.Insert(new { Title = "Test", Slug = "Test", Price = 12.00 });
            var c = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User" });
            Assert.IsFalse(DigitalRights.CanDownload(c, p));
        }

    }
}
