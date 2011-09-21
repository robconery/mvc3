using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Net;
using VidPub.Web.Model;
using System.Threading;

namespace VidPub.Tests.Functionals {
    
    [TestFixture]
    public class ShopifyTests {
        Orders _orders;
        OrderItems _items;
        public ShopifyTests() {
            _orders = new Orders();
            _items = new OrderItems();

        }
        string GetJson() {
            var jsonFile = @"C:\@Tekpub\VidPub\Source\VidPub.Web\App_Data\ShopifyPing.js";
            var result = "";
            using (var stream = new StreamReader(jsonFile)) {
                result = stream.ReadToEnd();
            }
            return result;
        }
        void Ping(string url, string data) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/json";

            //add the form data
            byte[] byteArray = Encoding.UTF8.GetBytes(data);
            request.ContentLength = byteArray.Length;

            using (var stream = request.GetRequestStream()) {
                // Write the data to the request stream.
                stream.Write(byteArray, 0, byteArray.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            string result = "";

            using (Stream stream = response.GetResponseStream()) {
                StreamReader sr = new StreamReader(stream);
                result = sr.ReadToEnd();
                sr.Close();
            }
        }
        [SetUp]
        public void Init() {

            _items.Delete();
            _orders.Delete();
        }
        [Test]
        public void receiver_should_save_one_order() {
            var url = "http://localhost:1701/shopify/receiver";
            Ping(url, GetJson());
            Assert.AreEqual(1, _orders.All().Count());
        }
        [Test]
        public void receiver_should_save_two_order_items() {
            var url = "http://localhost:1701/shopify/receiver";
            Ping(url, GetJson());
            Assert.AreEqual(2, _items.All().Count());
        }
    }
}
