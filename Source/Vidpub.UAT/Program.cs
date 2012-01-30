using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quixote;

namespace Vidpub.UAT {
    class Program {
        
        //TODO: Add a whole lot more tests! Like Registration, anon browsing of home and about,etc
        static void Main(string[] args) {

            Runner.SiteRoot = "http://localhost:1701";

            TheFollowing.Describes("The Home Page");

            It.Should("Have VidPub in the Title", () => {
                return Runner.Get("/").Title.ShouldContain("VidPub");
            });

            It.Should("Log me in with correct credentials", () => {
                var post= Runner.Post("/account/logon", new { login = "rob@tekpub.com", password = "password" });
                Console.WriteLine(post.Html);
                return post.Title.ShouldContain("Welcome");
            });

            Console.Read();

        }
    }
}
