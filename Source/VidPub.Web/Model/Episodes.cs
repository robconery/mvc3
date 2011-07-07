using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Episodes:DynamicModel {
        public Episodes():base("VidPub","Episodes","ID") {

        }
    }
}