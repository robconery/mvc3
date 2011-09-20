using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Massive;
using VidPub.Web.Model;
namespace VidPub.Tests.Reports {
    [TestFixture]
    public class VideoLogSpecs:TestBase {
        VideoLog _videoLog;
        [SetUp]
        public void Init() {
            _videoLog = new VideoLog();
        }

        [Test]
        public void sales_report_should_be_able_to_track_download_costs() {
            //need to be able to track a purchase to the log table so we can
            //reconcile it in the rollups...
            var item = _videoLog.Prototype;
            Assert.True(_videoLog.ItemContainsKey("OrderItemID", item));
            Assert.True(_videoLog.ItemContainsKey("BandwidthRate", item));
        }
    }
}
