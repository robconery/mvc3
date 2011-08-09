using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

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
        public override void Validate(dynamic item) {
            this.ValidatesPresenceOf(item.Title, "Title is required");
            this.ValidatesPresenceOf(item.Price, "Price is required");
            this.ValidateIsCurrency(item.Price, "Should be a number");
            //price needs to be > 0
            if (decimal.Parse(item.Price) <= 0) {
                Errors.Add("Price has to be more than 0 - can't give this stuff away!");
            }
        }
    }
}