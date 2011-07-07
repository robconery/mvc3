using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VidPub.Tests {
    //A Subscription grants access over time
    //there is a monthly and a yearly
    //a monthly subscription offers stream only access
    //a yearly offers stream and download
    //a Customer can buy a subscription

    [TestFixture]
    public class SubscriptionSpecs : TestBase {

        [Test]
        public void a_subscription_is_only_valid_one_per_user() {
            this.IsPending();
        }

        [Test]
        public void a_subscription_can_be_pending_current_overdue_suspended_or_cancelled() {
            this.IsPending();
        }

        [Test]
        public void a_pending_subscription_can_turn_current() {
            this.IsPending();
        }

        [Test]
        public void a_current_subscription_can_become_overdue_if_payment_late() {
            this.IsPending();
        }

        [Test]
        public void an_overdue_subscription_can_become_current_if_payment_received_in_full() {
            this.IsPending();
        }

        [Test]
        public void an_overdue_subscription_can_become_suspended_if_payment_not_received_after_3_tries() {
            this.IsPending();
        }


        [Test]
        public void a_subscription_can_be_upgraded_from_monthly_to_annual() {
            this.IsPending();
        }

        [Test]
        public void a_subscription_cannot_be_downgraded_from_annual_to_monthly() {
            this.IsPending();
        }
		

		
		
		
		
		
    }
}
