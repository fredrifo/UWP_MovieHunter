using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MovieHunter.DataAccessCore.Client.Models
{
    public class Validator
    {

        private const string _alg = "HmacSHA256"; //Generates an authentication code using SHA256 encrypeted hash function
        private const string _salt = "k0JnswhoocNGA7kCpyNY"; //Random generated salt value

        public static string GenerateToken(string username, string password, DateTime timestamp, int userId)
        {
            

            string hash = username + timestamp.ToString() + password;
            string firstHash = "";
            string secondHash = "";
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(password);
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));
                firstHash = Convert.ToBase64String(hmac.Hash);
                secondHash = username + ":" + timestamp.ToString();
            }

            //Validator(userId, timestamp, Convert.ToBase64String(Encoding.UTF8.GetBytes(firstHash + ":" + secondHash)));
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(firstHash + ":" + secondHash));

            //fix by creating tokens collection in startup new Token liveTokens = Tokens...
            //Tokens.Add(new Token { UserId = userId, Timestamp = timestamp, ValidationToken = token });
            return token;
        }
    }
}
