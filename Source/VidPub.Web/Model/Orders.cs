using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Orders:DynamicModel {
        public Orders():base("Vidpub","Orders","ID","OrderNumber") {

        }
    }
    public class OrderItems : DynamicModel {
        public OrderItems()
            : base("Vidpub", "OrderItems", "ID", "Name") {

        }
    }
}