using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using VidPub.Web.Model;

namespace VidPub.Tests.Reports {
    [TestFixture]
    public class ProductionReportingSpecs:TestBase {
        dynamic _productions;
        dynamic _episodes;
        [SetUp]
        public void Init() {
            _productions = new Productions();
            _episodes = new Episodes();
        }

        [Test]
        public void production_should_have_costing_fields() {
            var e = _episodes.First();
            var dc = (IDictionary<string, object>)e;
            Assert.True(dc.ContainsKey("DurationMinutes"));
        }
        [Test]
        public void production_should_have_fileSize_for_hd_stream_mobile_tablet() {
            var e = _episodes.First();
            var dc = (IDictionary<string, object>)e;
            Assert.True(dc.ContainsKey("HDFileSize"));
            Assert.True(dc.ContainsKey("MobileFileSize"));
            Assert.True(dc.ContainsKey("TabletFileSize"));
            Assert.True(dc.ContainsKey("StreamFileSize"));
        }

    }
}
