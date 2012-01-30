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
            return this.Query(@"select ID, First, Last, Email, HighriseID from customers
                            where email LIKE('%'+@0+'%')
                            or first LIKE ('%'+@0+'%')
                            or last LIKE ('%'+@0+'%')", query);
        }

        public dynamic OrderReived(dynamic order) {
            //see if the customer is in our system
            var customer = this.Single(where: "Email = @0", args:order.email);
            if (customer == null) {
                //create them
                customer = Insert(new {
                    First = order.billing_address.first_name,
                    Last = order.billing_address.last_name,
                    Email = order.email,
                });
            }

            //TODO: Harness up a Message Queue system - like RabbitMQ or ZeroMQ to handle these things transactionally.
            //Simply handing off to Task.Factory (in the TPL) is not good enough - we need failover and queueing

            //send a thank you?
            //var mimi = new MadMimi.Api("my@username.com", "MY_API_KEY");
            //var result = mimi.SendMailer("thanks", customer.Email, replacements: new { orderNumber = order.Number });

            //record the order with Highrise - the new record will have been added with the Insert above
            //var _highrise = new Highrise.Api(HQ_KEY, "YOUR DOMAIN");
            //_highrise.AddNote(customer.First, customer.Last, "Purchased Order " + order.name);
            return customer;
        }

        public override bool BeforeSave(dynamic item) {
            DefaultTo("StreamUntil", DateTime.Today.AddYears(-10), item);
            DefaultTo("DownloadUntil", DateTime.Today.AddYears(-10), item);
            return true;
        }

        public override void Inserted(dynamic item) {
            //add to Highrise
            //var api = new Highrise.Api(HQ_KEY, "YOUR DOMAIN");
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