using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Massive;

namespace VidPub.Web.Model {
    public static class DigitalRights {
        public static dynamic AuthTable {
            get{
                return new DynamicModel("Vidpub", "Customers_Productions");
            }
        }

        public static void AuthorizeOrder(dynamic customer, dynamic order) {
            dynamic itemTable = new OrderItems();
            dynamic productionsTable = new Productions();
            dynamic customersTable = new Customers();
            //loop the items and set auth accordingly
            foreach (var item in itemTable.Find(OrderID:order.ID)) {
                if (item.SKU == "monthly") {
                    //bump the customer's streaming
                    if (customer.StreamUntil < DateTime.Today.AddMonths(-1))
                        customer.StreamUntil = DateTime.Today.AddMonths(-1);
                    customer.StreamUntil = customer.StreamUntil.AddMonths(1);
                    customersTable.Update(customer,customer.ID);
                } else if (item.SKU == "yearly") {
                    if (customer.StreamUntil < DateTime.Today.AddYears(-1))
                        customer.StreamUntil = DateTime.Today.AddYears(-1);

                    if (customer.DownloadUntil < DateTime.Today.AddYears(-1))
                        customer.DownloadUntil = DateTime.Today.AddYears(-1);

                    customer.StreamUntil = customer.StreamUntil.AddYears(1);
                    customer.DownloadUntil = customer.DownloadUntil.AddYears(1);
                    customersTable.Update(customer,customer.ID);
                } else {
                    Authorize(customer, productionsTable.First(Slug: item.SKU));
                }
            }

        }

        public static bool CanDownload(dynamic customer, dynamic production) {
            if (production.Status != "released")
                return false;
            if (customer.DownloadUntil >= DateTime.Today)
                return true;
            if (IsAuthorized(customer, production))
                return true; 
            //default is no
            return false;
        }
        public static bool CanStream(dynamic customer, dynamic production) {
            if (production.Status != "released")
                return false;

            if (customer.StreamUntil >= DateTime.Today)
                return true;
            
            if (IsAuthorized(customer, production))
                return true; 

            //default is no
            return false;
        }

        public static void Authorize(dynamic customer, dynamic production) {
            //Yeah buddy... SQL!!!!!
            var sql = @"
            IF NOT EXISTS(Select CustomerID FROM Customers_Productions WHERE CustomerID=@0 AND ProductionID=@1) 
                BEGIN               
                    INSERT INTO Customers_Productions (CustomerID, ProductionID) VALUES (@0, @1)
                END";
            DB.Current.Execute(sql, customer.ID, production.ID);
        }
        public static void Revoke(dynamic customer, dynamic production) {
            var sql = @"DELETE FROM Customers_Productions WHERE CustomerID=@0 AND ProductionID=@1";
            DB.Current.Execute(sql, customer.ID, production.ID);
        }
        public static bool IsAuthorized(dynamic customer, dynamic production) {
            return AuthTable.Count(CustomerID: customer.ID, ProductionID: production.ID) > 0;
        }
    }
}