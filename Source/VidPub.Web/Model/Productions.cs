using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Productions:DynamicModel {
        public Productions():base("VidPub","Productions","ID") {

        }
    }
}