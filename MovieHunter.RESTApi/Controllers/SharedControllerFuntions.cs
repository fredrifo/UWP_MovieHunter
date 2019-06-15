using MovieHunter.DataAccessCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieHunter.RESTApi.Controllers
{
    public class SharedControllerFuntions
    {


        /// <summary>
        /// Tokens verificator. Cheks the token against the database. If the token exists, return the userId.
        /// This UserId is used by the api to decide if it the database rows belong to the user.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>The user id as a Nullable int.</returns>
        public static Nullable<int> TokenVerificator(string token)
        {
            FredrifoContext _context = new FredrifoContext();

            //TODO: check if the token is valid  var list = _context.TokenValidator.Where(d => d.Token == token && d.ValidFrom,...);
            var list = _context.TokenValidator.Where(d => d.Token == token);
            User tokenOwner = new User();

            if (list == null)
            {
                //The token does not excist in the database
                return null;
            }

            //The list should contain 1 item. if it contains multiple items it will use the last one.
            foreach (TokenValidator u in list)
            {
                tokenOwner.UserId = u.UserId;
            }

            //The token excists. 
            return tokenOwner.UserId;
        }
    }
}
