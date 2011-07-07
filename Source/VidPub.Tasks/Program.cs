using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Massive;
using VidPub.Web.Model;

namespace VidPub.Tasks {
    class Program {
        static void Main(string[] args) {
            var p = new Productions();
            p.All(where: "Title=@1", args: "thing","beef");
            //MyMethod(bop: "hi", stuff: "me");
        }
        static void MyMethod(string bop="", params object[] stuff){
            Console.WriteLine(stuff.Length);
        }
    }
}
