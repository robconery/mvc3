using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WatiN.Core;

namespace VidPub.Tests.Functionals {
    [TestFixture]
    [RequiresSTA] 
    public class HomePage:TestBase {

        [Test]
        public void home_page_should_have_vidpub_name() {

            using (var browser = new IE("http://localhost:1701")) {
                Assert.True(browser.Title.Contains("VidPub"));
            }

        }

    }
}
