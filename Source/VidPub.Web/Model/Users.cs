using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using Massive;
using System.Data.SqlServerCe;
using System.Security.Cryptography;

namespace VidPub.Web.Model {
    public class Users:DynamicModel {

        public Users():base("Membership", "Users", "ID") {}

        public dynamic Register(string email, string password, string confirm) {
            dynamic result = new ExpandoObject();
            result.Success = false;
            if (email.Length >= 6 && password.Length >= 6 && password.Equals(confirm)) {
                try {
                    result.UserID = this.Insert(new { Email = email, HashedPassword = Hash(password) });
                    result.Success = true;
                    result.Message = "Thanks for signing up!";
                } catch (SqlCeException ex) {
                    result.Message = "This email already exists in our system";
                }
            } else {
                result.Message = "Please check your email and password - they're invalid";
            }
            return result;
        }
        public static string Hash(string userPassword) {
            return
                BitConverter.ToString(SHA1Managed.Create().ComputeHash(Encoding.Default.GetBytes(userPassword))).Replace
                    ("-", "");
        }

        public void SetToken(string token, dynamic id) {
            this.Update(new { Token = token }, id);
        }


        public dynamic Login(string email, string password) {
            dynamic result = new ExpandoObject();

            result.User = this.Single("email  = @0 AND hashedpassword = @1", email, Hash(password));
            result.Authenticated = result.User != null;
            
            if (!result.Authenticated)
                result.Message = "Invalid email or password";
            
            return result;
        }


        public static dynamic FindByToken(string token) {
            var db = new Users();
            return db.Single(where: "Token = @0", args: token);
        }
    }
}
