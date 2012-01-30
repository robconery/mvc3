using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;
using System.ComponentModel.DataAnnotations;

namespace VidPub.Web.Model {
    public class Productions:DynamicModel {
        public Productions():base("VidPub","Productions","ID") {

        }
        public dynamic FuzzySearch(string query) {
            return this.Query(@"select ID, Title from productions
                            where title LIKE('%'+@0+'%')
                            or description LIKE('%'+@0+'%')
                            or slug LIKE('%'+@0+'%')
                            ", query);
        }

        public override bool BeforeSave(dynamic item) {
            //a status should be present
            if (!this.ItemContainsKey("Status", item))
                item.Status = "pending";
            return true;
        }
        
       

        public override void Validate(dynamic item) {
            if (item.Price <= 0)
                Errors.Add("Price can't be negative");
        }



        public IEnumerable<dynamic> Episodes(int productionID) {
            dynamic eps = new Episodes();
            return eps.Find(ProductionID: productionID);
        }

    }
}