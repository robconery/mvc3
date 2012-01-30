using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Episodes:DynamicModel {
        public Episodes():base("VidPub","Episodes","ID") {

        }
        public dynamic FuzzySearch(string query) {
            return this.Query(@"select Title, ProductionID as ID from episodes
                            where title LIKE('%'+@0+'%')
                            or description LIKE('%'+@0+'%')
                            ", query);
        }

    }
}