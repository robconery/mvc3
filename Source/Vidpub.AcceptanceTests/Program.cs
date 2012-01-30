using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quixote;

namespace Vidpub.AcceptanceTests {
    class Program {
        static void Main(string[] args) {
            Runner.SiteRoot = "http://localhost:1701/";
            TheFollowing.Describes("Home Page");

            It.Should("Have 'VidPub' in the title", () => {
                return Runner.Get("/").Title.ShouldContain("VidPugb");
            });

            It.Should("Log me in with correct credentials", () => {
                var post = Runner.Post("/account/logon", new { login = "rob@tekpub.com", password = "password" });
                return post.Title.ShouldContain("Welcome");
            });

            Console.Read();
        }
    }
}
