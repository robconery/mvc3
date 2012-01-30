using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VidPub.Web.Model;

namespace VidPub.Tests.Specs {
    [TestFixture]
    public class SearchSpecs:TestBase {

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
        
            //add some productions
            var p1 = _productions.Insert(new { Title = "Test", Slug = "p1", Price = 12.00 });
            var p2 = _productions.Insert(new { Title = "Test1", Slug = "p2", Price = 12.00 });
            var p3 = _productions.Insert(new { Title = "Lorem", Slug = "p3", Price = 12.00 });
            var p4 = _productions.Insert(new { Title = "Ipsem", Slug = "p4", Price = 12.00 });

            //episodes
            _episodes.Insert(new { Title = "Test2", Number = 1, ProductionID = p1.ID });
            _episodes.Insert(new { Title = "Test21", Number = 1, ProductionID = p1.ID });
            _episodes.Insert(new { Title = "Test3", Number = 1, ProductionID = p1.ID });
            _episodes.Insert(new { Title = "Test4", Number = 1, ProductionID = p1.ID });

            //customers
            var c1 = _customers.Insert(new { Email = "test@test.com", First = "Test", Last = "User" });
            var c2 = _customers.Insert(new { Email = "test1@test.com", First = "Joe", Last = "User"});
            var c3 = _customers.Insert(new { Email = "test2@test.com", First = "Joe", Last = "Loser"});
            var c4 = _customers.Insert(new { Email = "test21@test.com", First = "Test", Last = "Loser"});

        }

        [Test]
        public void production_search_should_return_1_for_p1_slug() {
            var result = (IEnumerable<dynamic>)_productions.FuzzySearch("p1");
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public void production_search_should_return_2_for_Test() {
            var result = (IEnumerable<dynamic>)_productions.FuzzySearch("Test");
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void production_search_should_return_4_for_p() {
            var result = (IEnumerable<dynamic>)_productions.FuzzySearch("p");
            Assert.AreEqual(4, result.Count());
        }
        [Test]
        public void production_search_should_return_2_for_em() {
            var result = (IEnumerable<dynamic>)_productions.FuzzySearch("em");
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void episode_search_should_return_4_for_Test() {
            var result = (IEnumerable<dynamic>)_episodes.FuzzySearch("Test");
            Assert.AreEqual(4, result.Count());
        }
        [Test]
        public void episode_search_should_return_2_for_Test2() {
            var result = (IEnumerable<dynamic>)_episodes.FuzzySearch("Test2");
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void customer_search_should_return_2_for_Test2() {
            var result = (IEnumerable<dynamic>)_customers.FuzzySearch("test2");
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void customer_search_should_return_2_for_Loser() {
            var result = (IEnumerable<dynamic>)_customers.FuzzySearch("Loser");
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void customer_search_should_return_2_for_Joe() {
            var result = (IEnumerable<dynamic>)_customers.FuzzySearch("joe");
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public void customer_search_should_return_4_for_test() {
            var result = (IEnumerable<dynamic>)_customers.FuzzySearch("test");
            Assert.AreEqual(4, result.Count());
        }
    }
}
