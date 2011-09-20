using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public class Customers:DynamicModel {
        public Customers():base("VidPub","Customers","ID","Email") {

        }

        public dynamic FuzzySearch(string query) {
            return this.Query(@"select ID, email as Title from customers
                            where email LIKE('%'+@0+'%')", query);
        }
        public override void Inserted(dynamic item) {
            //add to Highrise
            //var api = new Highrise.Api(HQ_KEY, "tekpub");
            //var newPerson = api.AddPerson(item.First, item.Last, 
                //item.Email, 
                //item.Tags.Split(new char[]{','},StringSplitOptions.RemoveEmptyEntries));
            //update the highrise id
            //this.Execute("UPDATE Customers SET HighriseID=@0 WHERE ID=@1", newPerson["id"], item.ID);
            
        }
        public override void Deleted(dynamic item) {
            //remove from Highrise
            //var api = new Highrise.Api(HQ_KEY, "tekpub");
            //api.Destroy(item.HighriseID);
        }

    }
}