using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Massive;
using VidPub.Web.Model;

namespace VidPub.Tests {


    [TestFixture]
    public class AuthenticationSpecs:TestBase {
        Users _membership;
        public AuthenticationSpecs() {
            this.Describes("User Authentication");
            _membership = new Users();
        }
        
        [SetUp]
        public void Init() {
            _membership.Delete();
        }

        [Test]
        public void not_found_should_return_false() {
            var result = _membership.Login("test@test.com", "password");
            Assert.False(result.Authenticated);
        }
        [Test]
        public void not_found_should_return_message() {
            var result = _membership.Login("test@test.com", "password");
            Assert.AreEqual("Invalid email or password", result.Message);
        }
        [Test]
        public void found_should_return_true() {
            _membership.Register("test@test.com", "password", "password");
            var result = _membership.Login("test@test.com", "password");
            Assert.True(result.Authenticated);
        }
		
		

    }

    [TestFixture]
    public class RegistrationSpecs:TestBase {

        Users _membership;

        public RegistrationSpecs() {
            this.Describes("User Registration");
            _membership = new Users();
        }
        [SetUp]
        public void Init() {
            _membership.Delete();
        }
        [Test]
        public void registration_should_allow_valid_unique_email_password() {
            var result = _membership.Register("test@test.com", "password", "password");
            Assert.True(result.Success);        
        }
		

        [Test]
        public void registration_should_not_accept_email_with_lt_6_chars() {
            var result = _membership.Register("p", "password", "password");
            Assert.False(result.Success);
        }

        [Test]
        public void registration_should_not_accept_password_with_lt_6_chars() {
            var result = _membership.Register("test@test.com", "x", "x");
            Assert.False(result.Success);
        }
        [Test]
        public void registration_should_not_accept_mismatched_passwords() {
            var result = _membership.Register("test@test.com", "password1", "password2");
            Assert.False(result.Success);
        }
        [Test]
        public void invalid_registration_should_return_message() {
            var result = _membership.Register("test@test.com", "password1", "password2");
            Assert.AreEqual(result.Message, "Please check your email and password - they're invalid");
        }
        [Test]
        public void email_must_be_unique() {
            var result = _membership.Register("test@test.com", "password", "password");
            result = _membership.Register("test@test.com", "password", "password");
            Assert.False(result.Success);
        }

        [Test]
        public void duplicate_email_should_return_message() {
            var result = _membership.Register("test@test.com", "password", "password");
            result = _membership.Register("test@test.com", "password", "password");
            Assert.AreEqual(result.Message, "This email already exists in our system");
        }
		
        [Test]
        public void user_should_be_saved_on_register() {
            var result = _membership.Register("test@test.com", "password", "password");
            Assert.AreEqual(1, _membership.All().Count());
        }
    }
}
