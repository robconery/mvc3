using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VidPub.Tests {
    //A customer can buy access to a Production
    //A customer can buy a Subscription

    [TestFixture]
    public class CustomerSpecs:TestBase {

        [Test]
        public void a_user_should_be_able_to_add_production_to_cart() {
            this.IsPending();
        }

        [Test]
        public void a_user_that_owns_a_production_should_be_able_to_stream() {
            this.IsPending();
        }

        [Test]
        public void a_user_that_owns_a_production_should_be_able_to_download() {
            this.IsPending();
        }
		
        [Test]
        public void a_user_should_have_be_able_to_purchase_sub() {
            this.IsPending();
        }

        [Test]
        public void a_user_with_monthly_should_only_be_able_to_stream() {
            this.IsPending();
        }

        [Test]
        public void a_user_with_yearly_should_be_able_to_stream_and_download() {
            this.IsPending();
        }

        [Test]
        public void a_user_with_cancelled_sub_should_not_be_able_to_stream_or_download() {
            this.IsPending();
        }

        [Test]
        public void a_user_with_a_suspended_sub_should_not_be_able_to_stream_or_download() {
            this.IsPending();
        }

        [Test]
        public void a_user_with_overdue_sub_should_be_able_to_stream_or_download() {
            this.IsPending();
        }
        [Test]
        public void a_user_should_be_able_to_cancel_sub() {
            this.IsPending();
        }
		
		
    }
}
