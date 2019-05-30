using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MovieHunter.DataAccessCore.Models
{
    public class Validator
    {
        //Generates an authentication code using SHA256 encrypted hash function
        private const string _alg = "HmacSHA256";
        //Random generated salt value
        private const string _salt = "k0JnswhoocNGA7kCpyNY";


        /// <summary>
        /// Generates a token based on username, passwords and timestamp.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public static string GenerateToken(string username, string password, DateTime timestamp, int userId)
        {
            //Getting strings from parameter
            string hash = username + timestamp.ToString() + password;
            string firstHash = "";
            string secondHash = "";

            //Creating a 
            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(password);

                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                firstHash = Convert.ToBase64String(hmac.Hash);

                secondHash = username + ":" + timestamp.ToString();
            }
            //Adding hash values together
            var token = Convert.ToBase64String(Encoding.UTF8.GetBytes(firstHash + ":" + secondHash));

            //If this code was complete, it would be possible to decode it using the same rules.

            return token;
        }
    }
}
