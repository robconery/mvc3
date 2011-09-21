using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace VidPub.Tests {

    //A Production is a collection of Episodes
    //A Customer can buy a Production
    //A Customer cannot buy an individual episode

    [TestFixture]
    public class ProductionSpecs : TestBase {

        [Test]
        public void a_production_has_one_or_more_episodes() {
            this.IsPending();
        }

        [Test]
        public void a_production_can_cost_0_or_more_dollars() {
            this.IsPending();
        }

        [Test]
        public void a_production_can_be_in_production_published_suspended_or_offline() {
            this.IsPending();
        }

        [Test]
        public void a_production_is_viewable_if_not_offline() {
            this.IsPending();
        }

        [Test]
        public void a_production_can_be_downloaded_if_flagged() {
            this.IsPending();
        }


        [Test]
        public void episodes_can_be_released_offline_in_process() {
            this.IsPending();
        }
		
        [Test]
        public void episodes_are_viewable_if_released() {
            this.IsPending();
        }


        [Test]
        public void customers_can_see_notes_per_production() {
            this.IsPending();
        }

        [Test]
        public void customers_can_see_notes_per_episode() {
            this.IsPending();
        }

        [Test]
        public void customers_can_see_when_an_episode_and_production_was_released() {
            this.IsPending();
        }

        [Test]
        public void customers_can_see_who_authored_the_production() {
            this.IsPending();
        }

        [Test]
        public void customers_can_see_how_long_an_episode_is() {
            this.IsPending();
        }

        [Test]
        public void customers_can_see_total_duration_of_production() {
            this.IsPending();
        }
		
		
		
		
		
		
		
    }
}
