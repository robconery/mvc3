using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VidPub.Web.Model;

namespace VidPub.Tests.Reports {
    [TestFixture]
    public class CustomerReportingSpecs:TestBase {
        dynamic _customers;
        dynamic _users;
        [SetUp]
        public void Init() {
            _customers = new Customers();
            _users = new Users();
        }

        [Test]
        public void customer_should_have_banned_switch() {
            var c = _users.First();
            var dc = (IDictionary<string, object>)c;
            Assert.True(dc.Keys.Any(x => x == "IsBanned"));
        }

    }
}
